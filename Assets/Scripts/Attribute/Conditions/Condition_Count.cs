using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CountCondition;
/// <summary>
/// ����������������ö�٣�����ָ�������ľ������͡�
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
/// ���������������ض��¼������ﵽһ�������󴥷�������
/// CountTypeö�ٶ��������ּ������ͣ����У�Hit����������Attack�������ˣ�Damaged���ͻ�ɱ��Kill�����������ܻ����ӡ�
/// ����Ҫ ���ӵ��������Ҫ�ڹ��캯������Ӷ�Ӧ��case�����޸�countType = (CountType)Random.Range(0, 4);��4Ϊ�µ�����
/// count ��������threshold ������������Ĵ�����clearOnTrigger �������Ƿ������������
/// 2025-8-25 ���뻹û���ԣ�����ǵò���
/// </summary>
public class Condition_Count : ModifierCondition
{
    
    private CountType countType;
    public EntityEventType eventType;
    public int count;
    public int threshold;
    public bool clearOnTrigger; //�������Ƿ����������
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
