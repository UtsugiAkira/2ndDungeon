using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Principal;
using Unity.VisualScripting;
using UnityEngine;
/// <summary>
/// 定义了武器的基本属性。
/// 基本属性包含武器名称、类型（暂时没用）、攻击伤害值和攻击冷却时间（暂时也没用）
/// </summary>
[Serializable]
public class WeaponAttribute  //:PropertyAttribute
{
    public string weaponName; // 武器名称
    public string weaponType; // 武器类型
    public float attackDamage; // 攻击伤害值
    public static WeaponAttribute NormalWeaponAttribute = new WeaponAttribute("NormalWeapon", "Melee", 5f);
    public List<WeaponModifier> modifiers = new List<WeaponModifier>();// 武器的修饰符列表
    public List<IWeaponAffix> affixes = new List<IWeaponAffix>(); // 武器词缀列表

    #region Debug
    public List<AffixType> affixTypes = new List<AffixType>(); // 词缀类型列表，方便Debug
    #endregion

    //public float attackCooldown; // 攻击冷却时间
    public WeaponAttribute(string name, string type, float damage)
    {
        weaponName = name;
        weaponType = type;
        attackDamage = damage;
        //attackCooldown = cooldown;
    }
    public WeaponAttribute() {
        this.weaponName = NormalWeaponAttribute.weaponName;
        this.weaponType = NormalWeaponAttribute.weaponType;
        this.attackDamage = NormalWeaponAttribute.attackDamage;
    }
    public void SetAffixes(List<IWeaponAffix> newAffixes)
    {
        affixes = newAffixes;
        affixTypes.Clear();
        foreach (var affix in affixes)
        {
            affixTypes.Add(affix.CurrentType);
        }
    }
}

/// <summary>
/// 所有武器的基类，定义了武器的基本行为和属性。
/// 包含了武器的持有者、武器属性、以及特殊词条。声明了武器的攻击方法和特殊攻击方法、攻击判定与能否攻击等。实现了武器的初始化。
/// ※重要 受制于架构，OnAttack为抽象方法但正常情况下其内部应包含TriggerEvent(EntityEventType.OnAttack);
/// </summary>
public abstract class Weapon : MonoBehaviour  
{
    [SerializeField]protected float attackCooldown; // 攻击冷却时间,参与玩家的攻击行为

    public _GameEntity owner; // 武器的拥有者

    public List<AffixType> affixTypes = new List<AffixType>(); // 词缀类型列表，方便Debug
    [SerializeField]
        protected WeaponAttribute weaponAttribute; // 武器属性
    public WeaponAttribute WeaponAttribute => weaponAttribute;
    /// <summary>
    /// 攻击方法，执行攻击逻辑。
    /// </summary>
    public virtual void WeaponInit(WeaponAttribute attribute)
    {
        if (attribute == null)
        {
            Debug.LogWarning("Weapon Attribute is null, using default attribute.");
            weaponAttribute = WeaponAttribute.NormalWeaponAttribute;
        }
        else
        {
            weaponAttribute = attribute;
        }
        ModifierInit();
        AffixInit();
    }

    public virtual void WeaponUninit()
    {
        if (owner != null && weaponAttribute.modifiers != null)
        {
            foreach (var modifier in weaponAttribute.modifiers)
            {
                modifier.RemoveAttribute(owner);
            }
        }
    }

    private void ModifierInit()
    {
        if (weaponAttribute.modifiers != null || weaponAttribute.modifiers.Count != 0)
        {
            foreach (var modifier in weaponAttribute.modifiers)
            {
                modifier.Init(this);
            }
        }
    }
    /// <summary>
    /// 这个地方又bug不知道怎么回事，用GameWeapon提供affix的时候就没问题，但是用RuntimeGameWeapon就会出问题
    /// </summary>
    private void AffixInit()
    {
        if (weaponAttribute.affixes != null || weaponAttribute.affixes.Count != 0)
        {
            foreach (var affix in weaponAttribute.affixes)
            {
                affix.ApplyAffix(this);
                affixTypes.Add(affix.CurrentType);
            }
        }
    }

    public abstract void Attack(Vector2 position);

    public abstract void SpecialAttack(Vector2 position);
    public virtual bool CanAttack()
    {
        return owner != null && 
               Time.time >= owner.LastAttackTime + attackCooldown;
    }
    public virtual bool CanSpecialAttack()
    {
        return owner != null &&
               Time.time >= owner.LastAttackTime + attackCooldown;
    }

    public virtual void HandleHit(_GameEntity target)
    {
        target.Damage(weaponAttribute.attackDamage * owner.AttackDamageMuliplier); // 根据武器数值和拥有者的攻击力倍率计算伤害
        owner.eventCenter.TriggerEvent(EntityEventType.OnHit);
    }

    public virtual void HandleHit(_GameEntity target, float damage)
    {
        target.Damage(damage * owner.AttackDamageMuliplier ); // 根据武器数值和拥有者的攻击力倍率计算伤害
        owner.eventCenter.TriggerEvent(EntityEventType.OnHit);
    }
}
