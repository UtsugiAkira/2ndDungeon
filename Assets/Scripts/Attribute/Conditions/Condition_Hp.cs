using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;
/// <summary>
/// �������η��Ĵ�������������Ѫ���ٷֱȵ�����
/// hpThreshold: Ѫ���ٷֱ���ֵ (0��1֮��)��isAbove: �Ƿ�Ϊ������ֵ (true: ����, false: ����)
/// ���캯�����޲ι��캯��������������Ĳ���
/// �вι��캯�������ֶ����ò��������ޣ�
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
            //��hpThresholdС��1ʱ����������Ǵ��ڻ���С�ڣ���hpThresholdԽСʱ(����0.2)�����ж������и������Ϊ�٣�����������ΪС��Ŀ��Ѫ����
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
