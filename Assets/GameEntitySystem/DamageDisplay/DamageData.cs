using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 伤害数据类，包含伤害信息和相关属性。主要用于伤害处理和显示（数值及位置，暴击等特效也可在后续实装）。
/// 同时带了一个调试信息字段，方便在调试时追踪伤害来源和类型。
/// </summary>
[System.Serializable]
public class DamageData
{
    public Vector3 targetPosition;
    public float amount;
    public bool isCritical;
    public bool isHeal;
    public string damageDebugMessage;
    public bool processingBreakFlag = false; //用于标记是否中断后续伤害处理
    public List<DamageModifierType> appliedModifiers;

    public DamageData(Vector3 position, float damageAmount, bool critical = false, bool heal = false,string message = "")
    {
        targetPosition = position;
        amount = damageAmount;
        isCritical = critical;
        isHeal = heal;
        processingBreakFlag = false;
        damageDebugMessage = message;
        appliedModifiers = new List<DamageModifierType>();
    }
}