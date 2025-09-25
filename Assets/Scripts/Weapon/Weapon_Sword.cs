using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// 定义了剑（挥动类）武器的属性类，继承自WeaponAttribute，增加了挥动相关的属性。
/// 后续需要随机算法生成slashAngle, slashRange, weaponSpeed等属性
/// 
/// </summary>
[Serializable]
public class SwordAttribute
{
    public float slashAngle  = 45f; // 单侧偏转角（度）
    public float slashRange  = 2f; // 距离（单位）
    public float weaponSpeed  = 12f; // 武器移动速度（单位/秒）
}

/// <summary>
/// 剑（挥动类）武器的属性类，继承自WeaponAttribute，增加了挥动相关的属性。实现了挥动武器的攻击效果。
/// 具体挥动方法见下文Slash Effect Function
/// 正常来说具有挥动效果的武器都可以继承此类并重写部分参数，挥动相关参数在SwordAttribute中定义
/// </summary>
[RequireComponent(typeof(Collider2D))] // 确保有碰撞体
public class Weapon_Sword : Weapon
{
    [SerializeField]private float spriteAngleOffset = 0f; // 精灵的初始角度偏移（度），根据美术资源调整

    [Header("Slash Settings")]
    protected Collider2D weaponCollider; // 武器的碰撞体
    protected SwordAttribute swordAttribute; // 武器属性
    #region slash effect
    bool _isAttacking = false;
    Transform _originalParent;
    Vector3 _originalLocalPos;
    Quaternion _originalLocalRot;
    #endregion

    private void Start()
    {
        Debug.Log("Sword Weapon Initialized");
        weaponCollider = GetComponent<Collider2D>();
        weaponCollider.isTrigger = true; // 确保武器碰撞体是触发器
        swordAttribute = new SwordAttribute(); // 初始化武器
    }

    public override void WeaponInit(WeaponAttribute attribute)
    {
        swordAttribute = new SwordAttribute();
        base.WeaponInit(attribute);
    }

    public override void Attack(Vector2 position)
    {
        /*Vector3 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouse.z = 0f;*/
        owner.eventCenter.TriggerEvent(EntityEventType.OnAttack);
        StartCoroutine(SwordAttack(position));
    }

    public override bool CanAttack()
    {
        return !_isAttacking;
    }

    public override void SpecialAttack(Vector2 position)
    {
        owner.eventCenter.TriggerEvent(EntityEventType.OnAttack);
        StartCoroutine(SpecialAttack(position));
    }

    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
        _GameEntity hitEntity;
        // 处理碰撞逻辑
        if (collision.gameObject.TryGetComponent(out hitEntity) && _isAttacking)
        {
            if (hitEntity != owner) // 确保不攻击自己
            {
                HandleHit(hitEntity);
            }
        }
    }

    #region Slash Effect Function
    public IEnumerator SwordAttack(Vector3 mouseWorldPos)
    {
        var attribute = swordAttribute;
        if (_isAttacking) yield break;
        _isAttacking = true;

        // 保存父对象和局部位置
        _originalParent = owner.transform;
        _originalLocalPos = transform.localPosition;
        _originalLocalRot = transform.localRotation;

        Vector3 playerPos = owner.transform.position;
        // 解除父子关系以便自由移动（用世界坐标操控）
        transform.SetParent(null);

        // 基准方向（玩家 -> 鼠标）
        Vector2 baseDir = (mouseWorldPos - playerPos);
        if (baseDir.sqrMagnitude < 1e-6f) baseDir = Vector2.right; // 防零向量
        float baseAngle = Mathf.Atan2(baseDir.y, baseDir.x) * Mathf.Rad2Deg;

        // 计算起始角与结束角（注意：顺时针 = 减角度，逆时针 = 加）
        Debug.Log(attribute==null);
        float startAngle = baseAngle + attribute.slashAngle; // 顺时针偏转 slashAngle 度
        float endAngle = baseAngle - attribute.slashAngle;   // 目标结束角

        // 计算绝对起始位置（世界坐标）
        Vector3 startPos = playerPos + AngleToVector(startAngle) * attribute.slashRange;

        // 飞到起点（平滑）
        yield return StartCoroutine(MoveToPosition(startPos, attribute.weaponSpeed * owner.AttackSpeedRate));
        transform.position = startPos; // 精确定位

        // 从 startAngle 开始，围绕 player 逆时针旋转 2*slashAngle 度，耗时 SlashTime 秒
        float rotated = 0f;
        float total = 2f * attribute.slashAngle;
        //float angularSpeed = total / Mathf.Max(0.0001f, SlashTime); // deg/sec
        float angularSpeed = attribute.weaponSpeed * owner.AttackSpeedRate* total /2;
        // 逐帧用 RotateAround 保持半径不变
        while (rotated < total - 1e-4f)
        {
            float delta = angularSpeed * Time.deltaTime;
            if (rotated + delta > total) delta = total - rotated;
            transform.RotateAround(playerPos, Vector3.forward, -delta); // Vector3.forward：2D 的 z 轴，正值为逆时针
            transform.Rotate(0,0,spriteAngleOffset); // 根据美术资源调整武器朝向
            rotated += delta;
            yield return null;
        }

        // 确保精确到结束角位置
        transform.position = playerPos + AngleToVector(endAngle) * attribute.slashRange;

        // 回到玩家身边指定的原始局部位置（world）
        Vector3 returnTargetWorld = owner.transform.TransformPoint(_originalLocalPos);
        
        yield return StartCoroutine(MoveToPosition(returnTargetWorld, attribute.weaponSpeed * owner.AttackSpeedRate));
        
        // 复位为玩家子物体，并设置局部位置
        transform.SetParent(_originalParent);
        transform.localPosition = _originalLocalPos;
        transform.localRotation = _originalLocalRot; // 恢复武器的局部旋转
        _isAttacking = false;
    }

    // 平滑移动到目标位置（世界坐标）
    IEnumerator MoveToPosition(Vector3 targetWorld, float speed)
    {
        const float threshold = 0.01f;
        while (Vector3.Distance(transform.position, targetWorld) > threshold)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetWorld, speed * Time.deltaTime);

            // 可选：让武器朝向移动方向（若你希望武器的朝向跟随）
            Vector3 toPlayer = transform.position - owner.transform.position;
            if (toPlayer.sqrMagnitude > 1e-6f)
                transform.up = toPlayer.normalized; // 在2D里通常用 up 指向目标方向

            yield return null;
        }
        transform.position = targetWorld;
    }

    // 角度（度）转单位向量（世界坐标）
    Vector3 AngleToVector(float deg)
    {
        float rad = deg * Mathf.Deg2Rad;
        return new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0f);
    }
    #endregion

    #region Special Attack Function

    public IEnumerator SpecialAttack(Vector3 mouseWorldPos)
    {
        var attribute = swordAttribute;
        if (_isAttacking) yield break;
        _isAttacking = true;

        // 保存父对象和局部位置
        _originalParent = owner.transform;
        _originalLocalPos = transform.localPosition;
        _originalLocalRot = transform.localRotation;

        Vector3 playerPos = owner.transform.position;
        // 解除父子关系以便自由移动（用世界坐标操控）
        transform.SetParent(null);

        // 基准方向（玩家 -> 鼠标）
        Vector2 baseDir = (mouseWorldPos - playerPos);
        if (baseDir.sqrMagnitude < 1e-6f) baseDir = Vector2.right; // 防零向量
        float baseAngle = Mathf.Atan2(baseDir.y, baseDir.x) * Mathf.Rad2Deg;

        // 计算起始角与结束角（注意：顺时针 = 减角度，逆时针 = 加）
        float startAngle = baseAngle + attribute.slashAngle; // 顺时针偏转 slashAngle 度
        float endAngle = baseAngle - attribute.slashAngle;   // 目标结束角

        // 计算绝对起始位置（世界坐标）
        Vector3 startPos = playerPos + AngleToVector(startAngle) * attribute.slashRange/2;

        // 飞到起点（平滑）
        yield return StartCoroutine(MoveToPosition(startPos, attribute.weaponSpeed * owner.AttackSpeedRate));
        transform.position = startPos; // 精确定位

        // 从 startAngle 开始，围绕 player 逆时针旋转 2*slashAngle 度，耗时 SlashTime 秒
        float rotated = 0f;
        float total = 360f;
        //float angularSpeed = total / Mathf.Max(0.0001f, SlashTime); // deg/sec
        float angularSpeed = attribute.weaponSpeed * owner.AttackSpeedRate * total / 2;
        // 逐帧用 RotateAround 保持半径不变
        while (rotated < total - 1e-4f)
        {
            float delta = angularSpeed * Time.deltaTime;
            if (rotated + delta > total) delta = total - rotated;
            transform.RotateAround(playerPos, Vector3.forward, -delta); // Vector3.forward：2D 的 z 轴，正值为逆时针
            rotated += delta;
            yield return null;
        }

        // 确保精确到结束角位置
        transform.position = playerPos + AngleToVector(endAngle) * attribute.slashRange;

        // 回到玩家身边指定的原始局部位置（world）
        Vector3 returnTargetWorld = owner.transform.TransformPoint(_originalLocalPos);

        yield return StartCoroutine(MoveToPosition(returnTargetWorld, attribute.weaponSpeed * owner.AttackSpeedRate));
        transform.localRotation = _originalLocalRot; // 恢复武器的局部旋转
        // 复位为玩家子物体，并设置局部位置
        transform.SetParent(_originalParent);
        transform.localPosition = _originalLocalPos;

        _isAttacking = false;
    }

    #endregion

}



