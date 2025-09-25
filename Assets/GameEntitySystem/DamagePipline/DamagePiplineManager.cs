using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// �˺�������߹�������Ŀǰ��ֻ��һ����̬����CalculateFinalDamage
/// CalculateFinalDamageͨ���������д����IDamageModifierʵ�������ε������ǵ�HandleDamage�������޸�baseDamage���ռ���������˺�ֵ
/// �����˺�ʱ�ᴫ��һ��DamageDataʵ�������ڹ����д��ݶ�����Ϣ���������԰����籩�����˺����͵���Ϣ�Ž�ȥ����Ӱ���˺���ֵ��ʾ
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
