using Mono.Cecil;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DM_UnHarmable : IDamageModifier
{
    public float HandleDamage(float damage, DamageData callbackData)
    {
        callbackData.processingBreakFlag = true;
        callbackData.damageDebugMessage += "\n<color=yellow>UnHarmable: No Damage Taken</color>";
        return 0;
    }
}
