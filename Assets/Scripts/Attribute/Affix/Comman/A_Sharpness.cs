using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A_Sharpness : IWeaponAffix
{
    public A_Sharpness()
    {
        isPercent = Random.Range(0, 2) == 1 ? true : false;
        if (isPercent)
            value = Random.Range(0.1f, 0.3f);
        else
            value = Random.Range(1f, 5f);
    }

    public override AffixType CurrentType { get => AffixType.Sharpness; }
    public override AffixType UnCompatibleAffixTypes { get => AffixType.Broken; }

    public override void ApplyAffix(Weapon weapon)
    {
        if (isPercent)
        {
            weapon.WeaponAttribute.attackDamage *= (1 - value);
        }
        else
        {
            weapon.WeaponAttribute.attackDamage -= value;
        }
    }
}
