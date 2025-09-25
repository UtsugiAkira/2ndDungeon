using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DamageModifierType
{
    None,
    Guard,      // 格挡，减半伤害
    Shield,     // 护盾，吸收伤害
    Thorns,     // 反伤，反弹部分伤害
    Vulnerable, // 易伤，增加受到的伤害
}

/// <summary>
/// 伤害修饰符接口，用于在伤害处理管线中修改伤害值
/// 实体在处理伤害时会遍历所有附加的IDamageModifier并调用HandleDamage方法
/// 基础伤害会依次传递给每个修饰符，最终返回修改后的伤害值
/// </summary>
public interface IDamageModifier 
{
    public abstract float HandleDamage(float damage,DamageData callbackData);
}
