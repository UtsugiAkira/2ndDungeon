using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Principal;
using Unity.VisualScripting;
using UnityEngine;
/// <summary>
/// �����������Ļ������ԡ�
/// �������԰����������ơ����ͣ���ʱû�ã��������˺�ֵ�͹�����ȴʱ�䣨��ʱҲû�ã�
/// </summary>
[Serializable]
public class WeaponAttribute  //:PropertyAttribute
{
    public string weaponName; // ��������
    public string weaponType; // ��������
    public float attackDamage; // �����˺�ֵ
    public static WeaponAttribute NormalWeaponAttribute = new WeaponAttribute("NormalWeapon", "Melee", 5f);
    public List<WeaponModifier> modifiers = new List<WeaponModifier>();// ���������η��б�
    public List<IWeaponAffix> affixes = new List<IWeaponAffix>(); // ������׺�б�

    #region Debug
    public List<AffixType> affixTypes = new List<AffixType>(); // ��׺�����б�����Debug
    #endregion

    //public float attackCooldown; // ������ȴʱ��
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
/// ���������Ļ��࣬�����������Ļ�����Ϊ�����ԡ�
/// �����������ĳ����ߡ��������ԡ��Լ���������������������Ĺ������������⹥�������������ж����ܷ񹥻��ȡ�ʵ���������ĳ�ʼ����
/// ����Ҫ �����ڼܹ���OnAttackΪ���󷽷���������������ڲ�Ӧ����TriggerEvent(EntityEventType.OnAttack);
/// </summary>
public abstract class Weapon : MonoBehaviour  
{
    [SerializeField]protected float attackCooldown; // ������ȴʱ��,������ҵĹ�����Ϊ

    public _GameEntity owner; // ������ӵ����

    public List<AffixType> affixTypes = new List<AffixType>(); // ��׺�����б�����Debug
    [SerializeField]
        protected WeaponAttribute weaponAttribute; // ��������
    public WeaponAttribute WeaponAttribute => weaponAttribute;
    /// <summary>
    /// ����������ִ�й����߼���
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
    /// ����ط���bug��֪����ô���£���GameWeapon�ṩaffix��ʱ���û���⣬������RuntimeGameWeapon�ͻ������
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
        target.Damage(weaponAttribute.attackDamage * owner.AttackDamageMuliplier); // ����������ֵ��ӵ���ߵĹ��������ʼ����˺�
        owner.eventCenter.TriggerEvent(EntityEventType.OnHit);
    }

    public virtual void HandleHit(_GameEntity target, float damage)
    {
        target.Damage(damage * owner.AttackDamageMuliplier ); // ����������ֵ��ӵ���ߵĹ��������ʼ����˺�
        owner.eventCenter.TriggerEvent(EntityEventType.OnHit);
    }
}
