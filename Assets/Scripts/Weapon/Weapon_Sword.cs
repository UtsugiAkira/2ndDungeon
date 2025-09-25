using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// �����˽����Ӷ��ࣩ�����������࣬�̳���WeaponAttribute�������˻Ӷ���ص����ԡ�
/// ������Ҫ����㷨����slashAngle, slashRange, weaponSpeed������
/// 
/// </summary>
[Serializable]
public class SwordAttribute
{
    public float slashAngle  = 45f; // ����ƫת�ǣ��ȣ�
    public float slashRange  = 2f; // ���루��λ��
    public float weaponSpeed  = 12f; // �����ƶ��ٶȣ���λ/�룩
}

/// <summary>
/// �����Ӷ��ࣩ�����������࣬�̳���WeaponAttribute�������˻Ӷ���ص����ԡ�ʵ���˻Ӷ������Ĺ���Ч����
/// ����Ӷ�����������Slash Effect Function
/// ������˵���лӶ�Ч�������������Լ̳д��ಢ��д���ֲ������Ӷ���ز�����SwordAttribute�ж���
/// </summary>
[RequireComponent(typeof(Collider2D))] // ȷ������ײ��
public class Weapon_Sword : Weapon
{
    [SerializeField]private float spriteAngleOffset = 0f; // ����ĳ�ʼ�Ƕ�ƫ�ƣ��ȣ�������������Դ����

    [Header("Slash Settings")]
    protected Collider2D weaponCollider; // ��������ײ��
    protected SwordAttribute swordAttribute; // ��������
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
        weaponCollider.isTrigger = true; // ȷ��������ײ���Ǵ�����
        swordAttribute = new SwordAttribute(); // ��ʼ������
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
        // ������ײ�߼�
        if (collision.gameObject.TryGetComponent(out hitEntity) && _isAttacking)
        {
            if (hitEntity != owner) // ȷ���������Լ�
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

        // ���游����;ֲ�λ��
        _originalParent = owner.transform;
        _originalLocalPos = transform.localPosition;
        _originalLocalRot = transform.localRotation;

        Vector3 playerPos = owner.transform.position;
        // ������ӹ�ϵ�Ա������ƶ�������������ٿأ�
        transform.SetParent(null);

        // ��׼������� -> ��꣩
        Vector2 baseDir = (mouseWorldPos - playerPos);
        if (baseDir.sqrMagnitude < 1e-6f) baseDir = Vector2.right; // ��������
        float baseAngle = Mathf.Atan2(baseDir.y, baseDir.x) * Mathf.Rad2Deg;

        // ������ʼ��������ǣ�ע�⣺˳ʱ�� = ���Ƕȣ���ʱ�� = �ӣ�
        Debug.Log(attribute==null);
        float startAngle = baseAngle + attribute.slashAngle; // ˳ʱ��ƫת slashAngle ��
        float endAngle = baseAngle - attribute.slashAngle;   // Ŀ�������

        // ���������ʼλ�ã��������꣩
        Vector3 startPos = playerPos + AngleToVector(startAngle) * attribute.slashRange;

        // �ɵ���㣨ƽ����
        yield return StartCoroutine(MoveToPosition(startPos, attribute.weaponSpeed * owner.AttackSpeedRate));
        transform.position = startPos; // ��ȷ��λ

        // �� startAngle ��ʼ��Χ�� player ��ʱ����ת 2*slashAngle �ȣ���ʱ SlashTime ��
        float rotated = 0f;
        float total = 2f * attribute.slashAngle;
        //float angularSpeed = total / Mathf.Max(0.0001f, SlashTime); // deg/sec
        float angularSpeed = attribute.weaponSpeed * owner.AttackSpeedRate* total /2;
        // ��֡�� RotateAround ���ְ뾶����
        while (rotated < total - 1e-4f)
        {
            float delta = angularSpeed * Time.deltaTime;
            if (rotated + delta > total) delta = total - rotated;
            transform.RotateAround(playerPos, Vector3.forward, -delta); // Vector3.forward��2D �� z �ᣬ��ֵΪ��ʱ��
            transform.Rotate(0,0,spriteAngleOffset); // ����������Դ������������
            rotated += delta;
            yield return null;
        }

        // ȷ����ȷ��������λ��
        transform.position = playerPos + AngleToVector(endAngle) * attribute.slashRange;

        // �ص�������ָ����ԭʼ�ֲ�λ�ã�world��
        Vector3 returnTargetWorld = owner.transform.TransformPoint(_originalLocalPos);
        
        yield return StartCoroutine(MoveToPosition(returnTargetWorld, attribute.weaponSpeed * owner.AttackSpeedRate));
        
        // ��λΪ��������壬�����þֲ�λ��
        transform.SetParent(_originalParent);
        transform.localPosition = _originalLocalPos;
        transform.localRotation = _originalLocalRot; // �ָ������ľֲ���ת
        _isAttacking = false;
    }

    // ƽ���ƶ���Ŀ��λ�ã��������꣩
    IEnumerator MoveToPosition(Vector3 targetWorld, float speed)
    {
        const float threshold = 0.01f;
        while (Vector3.Distance(transform.position, targetWorld) > threshold)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetWorld, speed * Time.deltaTime);

            // ��ѡ�������������ƶ���������ϣ�������ĳ�����棩
            Vector3 toPlayer = transform.position - owner.transform.position;
            if (toPlayer.sqrMagnitude > 1e-6f)
                transform.up = toPlayer.normalized; // ��2D��ͨ���� up ָ��Ŀ�귽��

            yield return null;
        }
        transform.position = targetWorld;
    }

    // �Ƕȣ��ȣ�ת��λ�������������꣩
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

        // ���游����;ֲ�λ��
        _originalParent = owner.transform;
        _originalLocalPos = transform.localPosition;
        _originalLocalRot = transform.localRotation;

        Vector3 playerPos = owner.transform.position;
        // ������ӹ�ϵ�Ա������ƶ�������������ٿأ�
        transform.SetParent(null);

        // ��׼������� -> ��꣩
        Vector2 baseDir = (mouseWorldPos - playerPos);
        if (baseDir.sqrMagnitude < 1e-6f) baseDir = Vector2.right; // ��������
        float baseAngle = Mathf.Atan2(baseDir.y, baseDir.x) * Mathf.Rad2Deg;

        // ������ʼ��������ǣ�ע�⣺˳ʱ�� = ���Ƕȣ���ʱ�� = �ӣ�
        float startAngle = baseAngle + attribute.slashAngle; // ˳ʱ��ƫת slashAngle ��
        float endAngle = baseAngle - attribute.slashAngle;   // Ŀ�������

        // ���������ʼλ�ã��������꣩
        Vector3 startPos = playerPos + AngleToVector(startAngle) * attribute.slashRange/2;

        // �ɵ���㣨ƽ����
        yield return StartCoroutine(MoveToPosition(startPos, attribute.weaponSpeed * owner.AttackSpeedRate));
        transform.position = startPos; // ��ȷ��λ

        // �� startAngle ��ʼ��Χ�� player ��ʱ����ת 2*slashAngle �ȣ���ʱ SlashTime ��
        float rotated = 0f;
        float total = 360f;
        //float angularSpeed = total / Mathf.Max(0.0001f, SlashTime); // deg/sec
        float angularSpeed = attribute.weaponSpeed * owner.AttackSpeedRate * total / 2;
        // ��֡�� RotateAround ���ְ뾶����
        while (rotated < total - 1e-4f)
        {
            float delta = angularSpeed * Time.deltaTime;
            if (rotated + delta > total) delta = total - rotated;
            transform.RotateAround(playerPos, Vector3.forward, -delta); // Vector3.forward��2D �� z �ᣬ��ֵΪ��ʱ��
            rotated += delta;
            yield return null;
        }

        // ȷ����ȷ��������λ��
        transform.position = playerPos + AngleToVector(endAngle) * attribute.slashRange;

        // �ص�������ָ����ԭʼ�ֲ�λ�ã�world��
        Vector3 returnTargetWorld = owner.transform.TransformPoint(_originalLocalPos);

        yield return StartCoroutine(MoveToPosition(returnTargetWorld, attribute.weaponSpeed * owner.AttackSpeedRate));
        transform.localRotation = _originalLocalRot; // �ָ������ľֲ���ת
        // ��λΪ��������壬�����þֲ�λ��
        transform.SetParent(_originalParent);
        transform.localPosition = _originalLocalPos;

        _isAttacking = false;
    }

    #endregion

}



