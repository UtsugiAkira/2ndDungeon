using UnityEngine;
using System.Collections.Generic;

public class FistWeapon : Weapon
{
    [Header("Fist Weapon Settings")]
    [SerializeField] private GameObject fistPrefab; // ȭͷԤ����
    [SerializeField] private int maxFists = 3; // ���ȭͷ����
    [SerializeField] private float fistSpeed = 10f; // ȭͷ�ƶ��ٶ�
    [SerializeField] private float spawnDistance = 1f; // ����λ�þ�����ҵľ���
    [SerializeField] private float fistDuration = 2f; // ȭͷ����ʱ��
    [SerializeField] private float horizontalSpread = 1f; // ˮƽɢ����Χ

    [SerializeField]private List<GameObject> activeFists = new List<GameObject>(); // ��ǰ��Ծ��ȭͷ�б�

    private void Start()
    {
    }

    public override bool CanAttack()
    {
        // �Ƴ������ٵ�ȭͷ����
        activeFists.RemoveAll(fist => fist == null);
        if (activeFists.Count < maxFists)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public override void Attack(Vector2 targetPosition)
    {
        // ����Ƿ�ﵽ���ȭͷ����
        if (activeFists.Count >= maxFists) return;

        // ���������¼�
        owner.eventCenter.TriggerEvent(EntityEventType.OnAttack);

        // ������󹥻�ʱ��
        owner.LastAttackTime = Time.time;

        // ��������λ�ã�������
        Vector2 spawnDirection = (owner.transform.position - (Vector3)targetPosition).normalized;
        Vector2 spawnPosition = (Vector2)owner.transform.position + spawnDirection * spawnDistance * Random.Range(0,1);
        spawnPosition +=  Vector2.Perpendicular(spawnDirection).normalized * horizontalSpread * Random.Range(-1, 1);
        // ����ȭͷ
        GameObject fist = Instantiate(fistPrefab, spawnPosition, Quaternion.identity);
        activeFists.Add(fist);

        // ����ȭͷ�ķ���
        FistController fistController = fist.GetComponent<FistController>();
        if (fistController != null)
        {
            fistController.Initialize(this, targetPosition, fistSpeed, fistDuration);
        }
        else
        {
            Debug.LogError("Fist prefab does not have FistController component!");
        }
    }

    public override void SpecialAttack(Vector2 position)
    {
        // ���⹥��ʵ�֣������Ҫ��
        // ���磺һ������������ȭͷ��������ǿȭͷ������
        Debug.Log("Special attack not implemented for Fist Weapon");
    }
}