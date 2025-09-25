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
/// 武器修饰符的触发条件基类，定义了触发条件的基本行为和属性。
/// EntityEventType TriggerType: 触发事件类型,用于指定修饰符监听的事件类型，在子类中实现
/// isConditionMet(_GameEntity entity): 判断条件是否满足的方法,用于具体实现条件逻辑，在子类中实现
/// 具体实例见Condition_Hp.cs
/// </summary>
public abstract class ModifierCondition
{
    public abstract EntityEventType TriggerType { get; }
    public abstract bool isConditionMet(_GameEntity entity);
}
/// <summary>
/// 武器修饰符的基类，定义了修饰符的基本行为和属性。通过继承本类，可以创建各种具体的武器修饰符。
/// hasProbability: 是否有概率触发 (默认false),probability: 触发概率 (0到1之间,默认1.0f),根据需要在子类构造函数中设置
/// weapon: 持有该修饰符的武器引用，用于访问武器和其拥有者的信息
/// condition: 触发条件的实例，用于判断修饰符是否应该触发,详见上述ModifierCondition类，modifierCondition为抽象类，实际存储的为其子类实例
/// Init(Weapon weapon): 初始化修饰符，传入持有该修饰符的武器引用,需在子类中实现并设置weapon和调用ApplyAttribute，可根据需求扩展
/// ※重要 Effect(): 定义修饰符触发时的效果,需在子类中实现
/// ApplyAttribute(_GameEntity entity): 将修饰符的属性应用到武器的，正常情况不需要重写
/// OnTrigger(): 触发修饰符的逻辑，检查条件并决定是否应用效果,正常情况不需要重写
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
