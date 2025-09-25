using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CountCondition;
/// <summary>
/// 基数类条件的类型枚举，用于指定条件的具体类型。
namespace CountCondition
{
    public enum CountType
    {
        Hit,
        Attack,
        Damaged,
        Kill,
    }
}
/// <summary>
/// 计数类条件，当特定事件发生达到一定次数后触发条件。
/// CountType枚举定义了四种计数类型：命中（Hit）、攻击（Attack）、受伤（Damaged）和击杀（Kill），后续可能会增加。
/// ※重要 增加的情况下需要在构造函数中添加对应的case，并修改countType = (CountType)Random.Range(0, 4);的4为新的数量
/// count 计数器，threshold 满足条件所需的次数，clearOnTrigger 触发后是否清零计数器。
/// 2025-8-25 代码还没测试，明天记得测试
/// </summary>
public class Condition_Count : ModifierCondition
{
    
    private CountType countType;
    public EntityEventType eventType;
    public int count;
    public int threshold;
    public bool clearOnTrigger; //触发后是否清零计数器
    public override EntityEventType TriggerType { get { return eventType; } }

    public Condition_Count() {
        countType = (CountType)Random.Range(0, 4);
        switch (countType)
        {
            case CountType.Hit:
                eventType = EntityEventType.OnHit;
                threshold = Random.Range(50, 200);
                break;
            case CountType.Attack:
                eventType = EntityEventType.OnAttack;
                threshold = Random.Range(100, 300);
                break;
            case CountType.Damaged:
                eventType = EntityEventType.OnInjury;
                threshold = Random.Range(1, 99);
                break;
            case CountType.Kill:
                eventType = EntityEventType.OnKill;
                threshold = Random.Range(10, 50);
                break;
            default:
                break;
        }
        count = 0;
    }

    public Condition_Count(CountType type ,int threshold, bool clearOnTrigger = true)
    {
        this.countType = type;
        this.threshold = threshold;
        this.clearOnTrigger = clearOnTrigger;
        this.count = 0;
        switch (countType)
        {
            case CountType.Hit:
                eventType = EntityEventType.OnHit;
                break;
            case CountType.Attack:
                eventType = EntityEventType.OnAttack;
                break;
            case CountType.Damaged:
                eventType = EntityEventType.OnInjury;
                break;
            case CountType.Kill:
                eventType = EntityEventType.OnKill;
                break;
            default:
                break;
        }
    }

    public override bool isConditionMet(_GameEntity entity)
    {
        if (count >= threshold)
        {
            if (clearOnTrigger) count = 0;
            return true;
        }
        else
        {
            count++;
            return false;
        }
    }
}
