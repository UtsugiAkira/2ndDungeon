using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AffixType
{
    #region Common
    Broken,
    Sharpness,
    #endregion

    #region Special
    ShootingSpeed,
    BulletSpeed,
    SwordRange,
    SwordSwingAngle,
    #endregion
}

/// <summary>
/// ������׺�ӿڣ��淶�����д�׺������
/// ��׺��������������ֱ���޸����������ԣ������ӹ������������ʵȣ�ͬʱ���⹥����ʽҲ����ͨ����׺��ʵ��
/// Ŀǰ�����������ڳ�ʼ��ʱ�������д�׺��������ApplyAffix������Ӧ�ô�׺Ч��
/// </summary>
public abstract class IWeaponAffix 
{
    protected float value;
    protected bool isPercent;
    public abstract AffixType CurrentType { get; }
    public abstract AffixType UnCompatibleAffixTypes { get; } // �ô�׺�ɼ��ݵ�������׺�����б�
    public abstract void ApplyAffix(Weapon weapon);
    public virtual bool IsCompatibleWith(IWeaponAffix otherAffix)
    {
        return otherAffix.CurrentType != UnCompatibleAffixTypes && otherAffix.CurrentType != this.CurrentType;
    }
}
