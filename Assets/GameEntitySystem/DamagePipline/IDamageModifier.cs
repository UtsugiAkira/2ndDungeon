using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DamageModifierType
{
    None,
    Guard,      // �񵲣������˺�
    Shield,     // ���ܣ������˺�
    Thorns,     // ���ˣ����������˺�
    Vulnerable, // ���ˣ������ܵ����˺�
}

/// <summary>
/// �˺����η��ӿڣ��������˺�����������޸��˺�ֵ
/// ʵ���ڴ����˺�ʱ��������и��ӵ�IDamageModifier������HandleDamage����
/// �����˺������δ��ݸ�ÿ�����η������շ����޸ĺ���˺�ֵ
/// </summary>
public interface IDamageModifier 
{
    public abstract float HandleDamage(float damage,DamageData callbackData);
}
