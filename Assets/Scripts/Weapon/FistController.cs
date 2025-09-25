using UnityEngine;
[RequireComponent(typeof(Collider2D))]
public class FistController : MonoBehaviour
{
    private Weapon ownerWeapon; // ��������
    private Vector2 targetPosition; // Ŀ��λ��
    private float speed; // �ƶ��ٶ�
    private float duration; // ����ʱ��
    private float timer; // ��ʱ��
    Collider2D collider;

    public void Initialize(Weapon weapon, Vector2 targetPos, float moveSpeed, float lifeTime)
    {
        ownerWeapon = weapon;
        targetPosition = targetPos;
        speed = moveSpeed;
        duration = lifeTime;
        timer = 0f;
        collider = GetComponent<Collider2D>();
        collider.isTrigger = true; // 

        // ����ȭͷ����Ŀ��
        Vector2 direction = (targetPosition - (Vector2)transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    private void Update()
    {
        // �ƶ�ȭͷ
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        // ���¼�ʱ��
        timer += Time.deltaTime;

        // ����Ƿ񵽴�Ŀ���ʱ
        if (Vector2.Distance(transform.position, targetPosition) < 0.1f || timer >= duration)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _GameEntity hitEntity;
        // ������ײ�߼�
        if (collision.gameObject.TryGetComponent(out hitEntity))
        {
            if (hitEntity != ownerWeapon.owner) // ȷ���������Լ�
            {
                ownerWeapon.HandleHit(hitEntity);
            }
        }
    }

}