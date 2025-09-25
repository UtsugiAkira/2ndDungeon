using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;

public class Weapon_RollingChair : Weapon_Sword
{
    // 必要字段
    [SerializeField] private float slideForce = 15f;
    [SerializeField] private float slideDuration = 2f;
    [SerializeField] private float knockbackForce = 10f;
    [SerializeField] private float minSlideSpeed = 1f;
    [SerializeField] private Collider2D slidingCollider; // 用于滑行时的碰撞检测
    private Vector2 originalLoaclPostion;   // 记录武器的初始位置
    private float lastFrameSpeed = 0f; // 记录上一次的速度
    private bool isSliding = false;
    private Vector2 slideDirection;

/*    private void Start()
    {
        slidingCollider.enabled = false; // 初始时关闭滑行碰撞体
    }*/

    // 技能入口：由玩家调用
    public override void SpecialAttack(Vector2 position)
    {
        if (isSliding) return;

        if (owner == null)
        {
            Debug.LogError("Weapon has no owner assigned.");
            return;
        }
        // 根据鼠标点击位置决定滑行方向
        slideDirection = (position - (Vector2)owner.transform.position).normalized;
        owner.StartCoroutine(SlideRoutine());
    }

    // 滑行逻辑：直接驱动玩家实体的 Rigidbody2D
    private IEnumerator SlideRoutine()
    {
        originalLoaclPostion = transform.localPosition;
        transform.localPosition = Vector2.zero; 
        isSliding = true;
        weaponCollider.enabled = false; // 关闭普通攻击碰撞体
        slidingCollider.enabled = true; // 启用滑行碰撞体
        float timer = 0f;
        owner.FreezeSelf();
        owner.rigid.AddForce(slideDirection * slideForce);
        owner.rigid.sharedMaterial  = new PhysicsMaterial2D(); // 创建新的物理材质
        owner.rigid.sharedMaterial.friction = 0f; // 减少摩擦力，保持滑行
        owner.rigid.sharedMaterial.bounciness = 0.99f; // 增加弹性，防止卡住
        lastFrameSpeed = owner.rigid.velocity.magnitude;
        while (timer < slideDuration )
        {
            float currentSpeed = owner.rigid.velocity.magnitude;
            if (currentSpeed < minSlideSpeed && currentSpeed - lastFrameSpeed < 0)
            {
                Debug.Log("Sliding ended early due to low speed.");
                break; // 速度过低，提前结束滑行
            }
            //owner.rigid.velocity = slideDirection * slideForce;
            timer += Time.deltaTime;
            lastFrameSpeed = currentSpeed;
            yield return null;
        }
        owner.rigid.sharedMaterial = null; // 恢复默认物理材质
        owner.rigid.velocity = Vector2.zero;
        isSliding = false;
        weaponCollider.enabled = true; // 恢复普通攻击碰撞体
        slidingCollider.enabled = false; // 关闭滑行碰撞体
        transform.localPosition = originalLoaclPostion;
        owner.DeFreezeSelf();
        // 技能结束 → 椅子回到手持状态（可选逻辑）
        // e.g. transform.SetParent(owner.handSocket);
    }

    public override void OnTriggerEnter2D(Collider2D collision)
    {
        if (isSliding)
        {

            _GameEntity hitEntity = collision.gameObject.GetComponent<_GameEntity>();
            // 1. 敌人 → 造成击退 + 伤害
            if (hitEntity != null && hitEntity != owner)
            {
                Vector2 knockDir = (collision.transform.position - owner.transform.position).normalized;
                hitEntity.rigid.AddForce(knockDir * knockbackForce, ForceMode2D.Impulse);

                // 调用敌人的受伤方法（统一走 DamagePipeline）
                hitEntity.Damage(weaponAttribute.attackDamage);
            }
        }
        base.OnTriggerEnter2D(collision);
        
    }

    public override bool CanAttack()
    {
        return !isSliding && base.CanAttack();
    }

    // 碰撞处理：在玩家对象的 OnCollisionEnter2D 中转发调用
    public void OnPlayerCollisionEnter2D(Collision2D collision)
    {
        

        // 2. 未来的放置物交互（注释预留）
        // var placedObj = collision.gameObject.GetComponent<IPlacedObject>();
        // if (placedObj != null) {
        //     placedObj.OnHitByChair(this);
        // }

        // 例：撞到立棍 → 转向
        // if (collision.gameObject.CompareTag("Stick")) {
        //     slideDirection = Vector2.Reflect(slideDirection, collision.contacts[0].normal).normalized;
        // }

        // 例：撞到椅子 → 触发范围攻击
        // if (collision.gameObject.CompareTag("Chair")) {
        //     TriggerAOEAttack(radius: 5f, damage: 20f);
        // }
    }

    // 示例：范围攻击（基于 _GameEntity）
    private void TriggerAOEAttack(float radius, float damage)
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(owner.transform.position, radius);
        foreach (Collider2D hit in hits)
        {
            _GameEntity entity = hit.GetComponent<_GameEntity>();
            if (entity != null && entity != owner)
            {
                entity.Damage(damage);
            }
        }
        Debug.Log($"范围攻击触发，半径={radius}, 伤害={damage}");
    }

}
