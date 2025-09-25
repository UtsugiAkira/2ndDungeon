using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;

public class Weapon_RollingChair : Weapon_Sword
{
    // ��Ҫ�ֶ�
    [SerializeField] private float slideForce = 15f;
    [SerializeField] private float slideDuration = 2f;
    [SerializeField] private float knockbackForce = 10f;
    [SerializeField] private float minSlideSpeed = 1f;
    [SerializeField] private Collider2D slidingCollider; // ���ڻ���ʱ����ײ���
    private Vector2 originalLoaclPostion;   // ��¼�����ĳ�ʼλ��
    private float lastFrameSpeed = 0f; // ��¼��һ�ε��ٶ�
    private bool isSliding = false;
    private Vector2 slideDirection;

/*    private void Start()
    {
        slidingCollider.enabled = false; // ��ʼʱ�رջ�����ײ��
    }*/

    // ������ڣ�����ҵ���
    public override void SpecialAttack(Vector2 position)
    {
        if (isSliding) return;

        if (owner == null)
        {
            Debug.LogError("Weapon has no owner assigned.");
            return;
        }
        // ���������λ�þ������з���
        slideDirection = (position - (Vector2)owner.transform.position).normalized;
        owner.StartCoroutine(SlideRoutine());
    }

    // �����߼���ֱ���������ʵ��� Rigidbody2D
    private IEnumerator SlideRoutine()
    {
        originalLoaclPostion = transform.localPosition;
        transform.localPosition = Vector2.zero; 
        isSliding = true;
        weaponCollider.enabled = false; // �ر���ͨ������ײ��
        slidingCollider.enabled = true; // ���û�����ײ��
        float timer = 0f;
        owner.FreezeSelf();
        owner.rigid.AddForce(slideDirection * slideForce);
        owner.rigid.sharedMaterial  = new PhysicsMaterial2D(); // �����µ��������
        owner.rigid.sharedMaterial.friction = 0f; // ����Ħ���������ֻ���
        owner.rigid.sharedMaterial.bounciness = 0.99f; // ���ӵ��ԣ���ֹ��ס
        lastFrameSpeed = owner.rigid.velocity.magnitude;
        while (timer < slideDuration )
        {
            float currentSpeed = owner.rigid.velocity.magnitude;
            if (currentSpeed < minSlideSpeed && currentSpeed - lastFrameSpeed < 0)
            {
                Debug.Log("Sliding ended early due to low speed.");
                break; // �ٶȹ��ͣ���ǰ��������
            }
            //owner.rigid.velocity = slideDirection * slideForce;
            timer += Time.deltaTime;
            lastFrameSpeed = currentSpeed;
            yield return null;
        }
        owner.rigid.sharedMaterial = null; // �ָ�Ĭ���������
        owner.rigid.velocity = Vector2.zero;
        isSliding = false;
        weaponCollider.enabled = true; // �ָ���ͨ������ײ��
        slidingCollider.enabled = false; // �رջ�����ײ��
        transform.localPosition = originalLoaclPostion;
        owner.DeFreezeSelf();
        // ���ܽ��� �� ���ӻص��ֳ�״̬����ѡ�߼���
        // e.g. transform.SetParent(owner.handSocket);
    }

    public override void OnTriggerEnter2D(Collider2D collision)
    {
        if (isSliding)
        {

            _GameEntity hitEntity = collision.gameObject.GetComponent<_GameEntity>();
            // 1. ���� �� ��ɻ��� + �˺�
            if (hitEntity != null && hitEntity != owner)
            {
                Vector2 knockDir = (collision.transform.position - owner.transform.position).normalized;
                hitEntity.rigid.AddForce(knockDir * knockbackForce, ForceMode2D.Impulse);

                // ���õ��˵����˷�����ͳһ�� DamagePipeline��
                hitEntity.Damage(weaponAttribute.attackDamage);
            }
        }
        base.OnTriggerEnter2D(collision);
        
    }

    public override bool CanAttack()
    {
        return !isSliding && base.CanAttack();
    }

    // ��ײ��������Ҷ���� OnCollisionEnter2D ��ת������
    public void OnPlayerCollisionEnter2D(Collision2D collision)
    {
        

        // 2. δ���ķ����ｻ����ע��Ԥ����
        // var placedObj = collision.gameObject.GetComponent<IPlacedObject>();
        // if (placedObj != null) {
        //     placedObj.OnHitByChair(this);
        // }

        // ����ײ������ �� ת��
        // if (collision.gameObject.CompareTag("Stick")) {
        //     slideDirection = Vector2.Reflect(slideDirection, collision.contacts[0].normal).normalized;
        // }

        // ����ײ������ �� ������Χ����
        // if (collision.gameObject.CompareTag("Chair")) {
        //     TriggerAOEAttack(radius: 5f, damage: 20f);
        // }
    }

    // ʾ������Χ���������� _GameEntity��
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
        Debug.Log($"��Χ�����������뾶={radius}, �˺�={damage}");
    }

}
