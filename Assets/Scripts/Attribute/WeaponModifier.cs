using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.Collections;
using UnityEngine;

public enum  Modifier
{
    Berserker,
}

/// <summary>
/// �������η��Ĵ����������࣬�����˴��������Ļ�����Ϊ�����ԡ�
/// EntityEventType TriggerType: �����¼�����,����ָ�����η��������¼����ͣ���������ʵ��
/// isConditionMet(_GameEntity entity): �ж������Ƿ�����ķ���,���ھ���ʵ�������߼�����������ʵ��
/// ����ʵ����Condition_Hp.cs
/// </summary>
public abstract class ModifierCondition
{
    public abstract EntityEventType TriggerType { get; }
    public abstract bool isConditionMet(_GameEntity entity);
}
/// <summary>
/// �������η��Ļ��࣬���������η��Ļ�����Ϊ�����ԡ�ͨ���̳б��࣬���Դ������־�����������η���
/// hasProbability: �Ƿ��и��ʴ��� (Ĭ��false),probability: �������� (0��1֮��,Ĭ��1.0f),������Ҫ�����๹�캯��������
/// weapon: ���и����η����������ã����ڷ�����������ӵ���ߵ���Ϣ
/// condition: ����������ʵ���������ж����η��Ƿ�Ӧ�ô���,�������ModifierCondition�࣬modifierConditionΪ�����࣬ʵ�ʴ洢��Ϊ������ʵ��
/// Init(Weapon weapon): ��ʼ�����η���������и����η�����������,����������ʵ�ֲ�����weapon�͵���ApplyAttribute���ɸ���������չ
/// ����Ҫ Effect(): �������η�����ʱ��Ч��,����������ʵ��
/// ApplyAttribute(_GameEntity entity): �����η�������Ӧ�õ������ģ������������Ҫ��д
/// OnTrigger(): �������η����߼�����������������Ƿ�Ӧ��Ч��,�����������Ҫ��д
/// </summary>
[System.Serializable]
public abstract class WeaponModifier
{
    public bool hasProbability = false;
    public float probability = 1.0f; // 0 to 1
    protected Weapon weapon;
    //protected EntityEventType triggerType;
    protected ModifierCondition condition;
    public abstract void Init(Weapon weapon);
    public virtual void ApplyAttribute(_GameEntity entity)
    {
        entity.eventCenter.AddEvent(condition.TriggerType, OnTrigger);
    }
    public virtual void RemoveAttribute(_GameEntity entity)
    {
        entity.eventCenter.RemoveEvent(condition.TriggerType, OnTrigger);
    }
    public virtual void OnTrigger()
    {
        Debug.Log("Modifier triggered: " + this.GetType().Name);
        if (condition.isConditionMet(weapon.owner))
        {
            if (hasProbability)
            {
                float roll = Random.Range(0f, 1f);
                if (roll <= probability)
                {
                    Effect();
                }
            }
            else
            {
                Effect();
            }
                
        }
    }



/*    public virtual bool Condition()
    {
        return true;
    }*/

    public abstract void Effect();
}
