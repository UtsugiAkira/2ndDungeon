using UnityEngine;
using System.Collections.Generic;

public class FistWeapon : Weapon
{
    [Header("Fist Weapon Settings")]
    [SerializeField] private GameObject fistPrefab; // 拳头预制体
    [SerializeField] private int maxFists = 3; // 最大拳头数量
    [SerializeField] private float fistSpeed = 10f; // 拳头移动速度
    [SerializeField] private float spawnDistance = 1f; // 生成位置距离玩家的距离
    [SerializeField] private float fistDuration = 2f; // 拳头存在时间
    [SerializeField] private float horizontalSpread = 1f; // 水平散布范围

    [SerializeField]private List<GameObject> activeFists = new List<GameObject>(); // 当前活跃的拳头列表

    private void Start()
    {
    }

    public override bool CanAttack()
    {
        // 移除已销毁的拳头引用
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
        // 检查是否达到最大拳头数量
        if (activeFists.Count >= maxFists) return;

        // 触发攻击事件
        owner.eventCenter.TriggerEvent(EntityEventType.OnAttack);

        // 更新最后攻击时间
        owner.LastAttackTime = Time.time;

        // 计算生成位置（玩家身后）
        Vector2 spawnDirection = (owner.transform.position - (Vector3)targetPosition).normalized;
        Vector2 spawnPosition = (Vector2)owner.transform.position + spawnDirection * spawnDistance * Random.Range(0,1);
        spawnPosition +=  Vector2.Perpendicular(spawnDirection).normalized * horizontalSpread * Random.Range(-1, 1);
        // 生成拳头
        GameObject fist = Instantiate(fistPrefab, spawnPosition, Quaternion.identity);
        activeFists.Add(fist);

        // 设置拳头的方向
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
        // 特殊攻击实现（如果需要）
        // 例如：一次性生成所有拳头，或者增强拳头威力等
        Debug.Log("Special attack not implemented for Fist Weapon");
    }
}