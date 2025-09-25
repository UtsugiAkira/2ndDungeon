using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// �˺������࣬�����˺���Ϣ��������ԡ���Ҫ�����˺��������ʾ����ֵ��λ�ã���������ЧҲ���ں���ʵװ����
/// ͬʱ����һ��������Ϣ�ֶΣ������ڵ���ʱ׷���˺���Դ�����͡�
/// </summary>
[System.Serializable]
public class DamageData
{
    public Vector3 targetPosition;
    public float amount;
    public bool isCritical;
    public bool isHeal;
    public string damageDebugMessage;
    public bool processingBreakFlag = false; //���ڱ���Ƿ��жϺ����˺�����
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