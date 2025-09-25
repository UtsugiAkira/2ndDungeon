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
/// 武器词缀接口，规范了所有词缀的内容
/// 词缀用于修饰武器以直接修改武器的属性，如增加攻击力、暴击率等，同时特殊攻击方式也可以通过词缀来实现
/// 目前设想中武器在初始化时遍历所有词缀并调用其ApplyAffix方法来应用词缀效果
/// </summary>
public abstract class IWeaponAffix 
{
    protected float value;
    protected bool isPercent;
    public abstract AffixType CurrentType { get; }
    public abstract AffixType UnCompatibleAffixTypes { get; } // 该词缀可兼容的其他词缀类型列表
    public abstract void ApplyAffix(Weapon weapon);
    public virtual bool IsCompatibleWith(IWeaponAffix otherAffix)
    {
        return otherAffix.CurrentType != UnCompatibleAffixTypes && otherAffix.CurrentType != this.CurrentType;
    }
}
