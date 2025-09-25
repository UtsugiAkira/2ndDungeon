using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;
/// <summary>
/// 武器修饰符的触发条件：基于血量百分比的条件
/// hpThreshold: 血量百分比阈值 (0到1之间)，isAbove: 是否为高于阈值 (true: 高于, false: 低于)
/// 构造函数：无参构造函数用于配置随机的参数
/// 有参构造函数用于手动设置参数（暂无）
/// </summary>
[System.Serializable]
public class Condition_Hp : ModifierCondition
{
    private float hpThreshold;
    private bool isAbove; // true: above threshold, false: below threshold

    public Condition_Hp()
    {
        hpThreshold = Random.Range(0f, 1.5f); // Random threshold between 20 and 80
        if(hpThreshold >= 1f)
        {
            isAbove = true;
        }else
        {
            //当hpThreshold小于1时，随机决定是大于还是小于，当hpThreshold越小时(例如0.2)下列判断条件有更大概率为假（即触发条件为小于目标血量）
            isAbove = Random.value < hpThreshold; // Randomly decide above or below
        }
    }

    public override EntityEventType TriggerType { get { return EntityEventType.OnHealthChanged; } }

    public override bool isConditionMet(_GameEntity entity)
    {
        if(isAbove)
        {
            return entity.Health/entity.MaxHealth >= hpThreshold;
        }
        else
        {
            return entity.Health / entity.MaxHealth <= hpThreshold;
        }
    }
}
