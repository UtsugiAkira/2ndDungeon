using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D),typeof(Collider2D),typeof(EntityEventCenter))]
public class _GameEntity : MonoBehaviour
{
    [SerializeField]
    private float _health;
    public float Health
    {
        get { return _health; }
        set
        {
            _health = value;
            DeathChecck();
        }
    }
    [SerializeField]
    private float _maxHealth;
    public float MaxHealth
    {
        get { return _maxHealth; }
        set { _maxHealth = value; }
    }
    private float _mp;
    public float Mp
    {
        get { return _mp; }
        set
        {
            _mp = value;
            if (_mp > _maxMp) _mp = _maxMp; // Ensure MP does not exceed max MP
        }
    }
    private float _maxMp;
    public float MaxMp
    {
        get { return _maxMp; }
        set { _maxMp = value; }
    }
    [SerializeField]
    private float _speed;
    public float speedMuliplier = 1f; // Speed multiplier for buffs/debuffs
    public float Speed
    {
        get { return _speed*speedMuliplier; }
        set { _speed = value; }
    }
    private float _AttackRate;
    public float AttackRate { get { return _AttackRate; } 
        set { _AttackRate = value; }
    }
    private float _AttackSpeedRate;
    public float AttackSpeedRate
    {
        get { return _AttackSpeedRate; }
        set { _AttackSpeedRate = value; }
    }
    private float attackDamageMuliplier = 1f; // Attack damage multiplier for buffs/debuffs
    public float AttackDamageMuliplier
    {
        get { return attackDamageMuliplier; }
        set { attackDamageMuliplier = value; }
    }

    private float lastAttackTime; // 上次攻击时间
    public float LastAttackTime { get { return lastAttackTime; } set { lastAttackTime = value; } }
    [SerializeField]
    private bool _isAlive;
    public bool IsAlive
    {
        get { return _isAlive; }
    }
    private bool _isgrounded;
    public bool IsGrounded
    {
        get { return _isgrounded; }
    }
    public Rigidbody2D rigid;
    public Animator anim;
    public FSM_Manager fsm; // 状态机管理器
    public Weapon currentWeapon; // 当前装备的武器
    public EntityEventCenter eventCenter;// 事件中心

    #region Damage Pipeline
    public List<IDamageModifier> damageModifiers = new List<IDamageModifier>(); // 伤害修正器列表
    public DamageData LastDamageData; // 记录最后一次伤害数据
    #endregion

    protected void DeathChecck()
    {
        if(_health <= 0)
        {
            _isAlive = false;
            _health = 0;
            // Handle death logic here, e.g., play death animation, disable entity, etc.
            Debug.Log("Entity is dead.");
        }
    }

    protected void EntityInit(float maxHealth, float maxMp, float speed)
    {
        _health = maxHealth;
        _maxHealth = maxHealth;
        _mp = maxMp;
        _maxMp = maxMp;
        _speed = speed;
        _isAlive = true;
        _AttackRate = 1f;
        _AttackSpeedRate = 1f;

        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
        eventCenter = GetComponent<EntityEventCenter>();
        //AnimateInit();
    }

    protected virtual void GroundCheck()
    {
        // Implement ground check logic here
        LayerMask groundLayer = LayerMask.GetMask("Ground");
        _isgrounded = Physics2D.Raycast(transform.position, Vector2.down, 0.1f, groundLayer);
    }

    public virtual void Damage(float damageAmount)
    {
        if (_isAlive)
        {
            LastDamageData = new DamageData(transform.position, damageAmount);
            _health -= DamagePiplineManager.CalculateFinalDamage(damageAmount,damageModifiers,LastDamageData);
            //当受到伤害时进入受伤状态并触发血量变化事件
            if (LastDamageData.amount > 0)
            {
                fsm?.SwitchState(StateType.Damaged); // Switch to damaged state if FSM is present
                eventCenter.TriggerEvent(EntityEventType.OnHealthChanged);
            }
            //即使没有受到伤害也触发受伤事件（可能是免疫伤害的buff）
            EventCenter.instance.TriggerEvent<DamageData>(EventType.EntityDamage, LastDamageData);
            eventCenter.TriggerEvent(EntityEventType.OnInjury);
            //Debug.Log(fsm);
            DeathChecck();
        }
    }

    public virtual void Damage(float damageAmount,DamageType type)
    {
        if (_isAlive)
        {
            _health -= damageAmount;
            fsm?.SwitchState(StateType.Damaged); // Switch to damaged state if FSM is present
            eventCenter.TriggerEvent(EntityEventType.OnInjury);
            eventCenter.TriggerEvent(EntityEventType.OnHealthChanged);
            DeathChecck();
        }
    }

/*    public virtual float HandleDamage(float damageAmount)
    {
        //string debugInfo = $"Initial Damage: {damageAmount}. ";
        //float finalDamage = damageAmount;
        *//*if(BuffManager.instance.HasBuff(this,BuffType.Guarding))
        {
            finalDamage *= 0.5f; // Reduce damage by 50% when guarding
            debugInfo += "\nGuarding active, damage reduced by 50%. ";
        }
        Debug.Log($"Final Damage Taken: {finalDamage}\n" + debugInfo);*//*

        return finalDamage;
    }*/

    public virtual void Heal(float healAmount)
    {
        if (_isAlive)
        {
            _health += healAmount;
            if (_health > _maxHealth)
                _health = _maxHealth;
            Debug.Log($"Entity healed: {healAmount}. Current health: {_health}");
            eventCenter.TriggerEvent(EntityEventType.OnHealthChanged);
            eventCenter.TriggerEvent(EntityEventType.OnHeal);
        }
    }

    public virtual void FreezeSelf()
    {
        fsm.SwitchState(StateType.Freeze);
    }

    public virtual void DeFreezeSelf()
    {
        fsm.SwitchState(StateType.Idle);
    }
    /*public void AnimateInit()
    {
        foreach (AnimatorClipInfo clip in anim.GetCurrentAnimatorClipInfo(0))
        {
            clip.clip.AddEvent(new AnimationEvent
            {
                functionName = "OnAnimationEnd",
                time = clip.clip.length // Adjust the time as needed
            });
        }
    }

    public void OnAnimationEnd()
    {
        fsm.SwitchState(StateType.Idle); // Switch to Idle state when animation ends
    }*/
}
