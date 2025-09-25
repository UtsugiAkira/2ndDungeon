using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 格挡修饰符，在伤害处理时将伤害减半
/// </summary>
public class DM_Guard : IDamageModifier
{
    public float HandleDamage(float damage,DamageData callbackData)
    {
        callbackData.damageDebugMessage += "\n<color=yellow>Guarding active, damage reduced by 50%.</color>";
        callbackData.appliedModifiers.Add(DamageModifierType.Guard);
        return damage / 2;
    }
}
