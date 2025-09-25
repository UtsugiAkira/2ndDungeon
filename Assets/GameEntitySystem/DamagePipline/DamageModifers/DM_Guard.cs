using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// �����η������˺�����ʱ���˺�����
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
