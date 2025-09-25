using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(Animator))]
public class _GamePlayer : _GameEntity
{ 
    public static _GamePlayer Instance; // 单例模式，方便其他类访问玩家实例
    public StateType currentState;
    public Slider hpBar; // 生命值UI
    public Slider mpBar; // 魔法值UI
    public InteractableObject interactTarget;

    protected void Awake()
    {
        if (Instance == null)
        {
            Instance = this; // 确保只有一个实例存在
        }
        else
        {
            Destroy(gameObject); // 如果已经有实例，销毁新的实例
            return;
        }
        fsm = new FSM_Manager(this); 
    }

    private void Start()
    {
        _PlayerWeaponSystem.Instance.SwitchWeapon(0); // 切换到初始武器
        EntityInit(100f, 50f, 2f); // 初始化玩家属性：生命值、魔法值、速度
        InitializeStates();
    }

    private void Update()
    {
        if (IsAlive)
        {
            //测试阶段先这么写，后续会改成事件驱动
            hpBar.value = Health / MaxHealth; // 更新生命值UI
            mpBar.value = Mp / MaxMp; // 更新魔法值UI
            fsm.OnUpdate(); // 更新状态机

            currentState = fsm.currentType;
            if (Input.GetMouseButtonDown(0))
            {
                if (currentWeapon != null && currentWeapon.CanAttack())
                {
                    currentWeapon.Attack(Camera.main.ScreenToWorldPoint(Input.mousePosition)); // 执行攻击
                    LastAttackTime = Time.time; // 更新上次攻击时间
                }
            }
            if (Input.GetMouseButtonDown(1) && currentWeapon.CanSpecialAttack())
            {
                if (currentWeapon != null)
                {
                    currentWeapon.SpecialAttack(Camera.main.ScreenToWorldPoint(Input.mousePosition)); // 执行特殊攻击
                    LastAttackTime = Time.time; // 更新上次攻击时间
                }
            }

            if(Input.GetKeyDown(KeyCode.Q))
            {
                _PlayerWeaponSystem.Instance.SwitchWeapon(-1); // 切换到上一个武器
            }
            else if(Input.GetKeyDown(KeyCode.E))
            {
                _PlayerWeaponSystem.Instance.SwitchWeapon(1); // 切换到下一个武器
            }
        }
    }

    private void FixedUpdate()
    {
        if (IsAlive)
        {
            fsm.OnFixedUpdate(); // 更新状态机的物理状态
        }
    }

    private void InitializeStates()
    {
        fsm = new FSM_Manager(this);
        fsm.AddState(StateType.Idle, new State_Player_Idle(fsm));
        fsm.AddState(StateType.Move, new State_Player_Move(fsm));
        fsm.AddState(StateType.Damaged, new State_player_Damaged(fsm));
        fsm.AddState(StateType.Dead, new State_Player_Dead(fsm));
        fsm.AddState(StateType.Freeze,new State_Special_EntityFreeze(fsm));
        fsm.SwitchState(StateType.Idle);
    }

}