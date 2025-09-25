using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 伤害处理管线管理器，目前就只有一个静态方法CalculateFinalDamage
/// CalculateFinalDamage通过遍历所有传入的IDamageModifier实例，依次调用它们的HandleDamage方法来修改baseDamage最终计算出最终伤害值
/// 处理伤害时会传入一个DamageData实例用于在管线中传递额外信息，后续可以把例如暴击、伤害类型等信息放进去用来影响伤害数值显示
/// </summary>
public class DamagePiplineManager
{

    public static float CalculateFinalDamage(float baseDamage, List<IDamageModifier> modifiers,DamageData data)
    {
        float finalDamage = baseDamage;
        if (modifiers == null || modifiers.Count == 0)
        {
            Debug.Log("No damage modifiers found. Final Damage: " + finalDamage);
            data = new DamageData(Vector3.zero, finalDamage);
            return finalDamage;
        }

        foreach (var modifier in modifiers)
        {
            finalDamage = modifier.HandleDamage(finalDamage,data);
            if(data.processingBreakFlag)
            {
                data.damageDebugMessage += $"Processing broken by {modifier.GetType().Name}. ";
                break;
            }
        }
        data.amount = finalDamage;
        Debug.Log(data.damageDebugMessage);
        return finalDamage;

    }

}
