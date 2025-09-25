using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanEnemy : _GameEntity
{
    public _GameEntity target; // Target for the enemy to approach
    public float approachDistance = 5f; // Distance at which the enemy will approach the target
    public float wanderRadius = 10f; // Radius within which the enemy will wander
    public float wanderCD = 2f; // Time CD for wandering
    public float wanderTimer = 0f; // Timer for wandering

    public _GameWeaponItem initWeapon; // Current weapon of the enemy


    // Start is called before the first frame update
    void Start()
    {
        EntityInit(100f, 50f, 1f); // Initialize entity with health, mana, and speed
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
        fsm = new FSM_Manager(this);
        fsm.AddState(StateType.Idle, new State_HumanEnemy_Idle(fsm));
        fsm.AddState(StateType.Wander, new State_HumanEnemy_Wander(fsm));
        fsm.AddState(StateType.Approch, new State_HumanEnemy_Approch(fsm));
        fsm.AddState(StateType.Damaged, new State_HumanEnemy_Damaged(fsm));
        fsm.SwitchState(StateType.Idle); // Start with the Idle state
        WeaponSystem.Instance.GenerateWeapon(initWeapon.GetRuntimeWeaponItem(), this); // Equip initial weapon
    }
    private void Update()
    {
        if (IsAlive)
        {
            fsm.OnUpdate(); // Update the FSM state
        }
        
    }
    public void FixedUpdate()
    {
        if (IsAlive)
        {
            fsm.OnFixedUpdate(); // Update the FSM state in fixed time
        }
    }   

    public bool SearchForTarget()
    {
        if ( Vector2.Distance(_GamePlayer.Instance.transform.position, transform.position) < approachDistance)
        {
            target = _GamePlayer.Instance; // Set the target to the player if within approach distance
            return true;
        }
        else
        {
            return false; // No target found
        }
    }

    public void Attack()
    {
        //下面要改一下，之后根据武器的攻击范围决定能不能攻击
        if (currentWeapon != null && currentWeapon.CanAttack() )
        {
            currentWeapon.Attack(target.transform.position); // Execute attack
            LastAttackTime = Time.time; // 更新上次攻击时间
        }
    }
}
