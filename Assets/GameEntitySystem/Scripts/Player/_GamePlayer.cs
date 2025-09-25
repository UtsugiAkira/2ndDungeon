using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(Animator))]
public class _GamePlayer : _GameEntity
{ 
    public static _GamePlayer Instance; // ����ģʽ������������������ʵ��
    public StateType currentState;
    public Slider hpBar; // ����ֵUI
    public Slider mpBar; // ħ��ֵUI
    public InteractableObject interactTarget;

    protected void Awake()
    {
        if (Instance == null)
        {
            Instance = this; // ȷ��ֻ��һ��ʵ������
        }
        else
        {
            Destroy(gameObject); // ����Ѿ���ʵ���������µ�ʵ��
            return;
        }
        fsm = new FSM_Manager(this); 
    }

    private void Start()
    {
        _PlayerWeaponSystem.Instance.SwitchWeapon(0); // �л�����ʼ����
        EntityInit(100f, 50f, 2f); // ��ʼ��������ԣ�����ֵ��ħ��ֵ���ٶ�
        InitializeStates();
    }

    private void Update()
    {
        if (IsAlive)
        {
            //���Խ׶�����ôд��������ĳ��¼�����
            hpBar.value = Health / MaxHealth; // ��������ֵUI
            mpBar.value = Mp / MaxMp; // ����ħ��ֵUI
            fsm.OnUpdate(); // ����״̬��

            currentState = fsm.currentType;
            if (Input.GetMouseButtonDown(0))
            {
                if (currentWeapon != null && currentWeapon.CanAttack())
                {
                    currentWeapon.Attack(Camera.main.ScreenToWorldPoint(Input.mousePosition)); // ִ�й���
                    LastAttackTime = Time.time; // �����ϴι���ʱ��
                }
            }
            if (Input.GetMouseButtonDown(1) && currentWeapon.CanSpecialAttack())
            {
                if (currentWeapon != null)
                {
                    currentWeapon.SpecialAttack(Camera.main.ScreenToWorldPoint(Input.mousePosition)); // ִ�����⹥��
                    LastAttackTime = Time.time; // �����ϴι���ʱ��
                }
            }

            if(Input.GetKeyDown(KeyCode.Q))
            {
                _PlayerWeaponSystem.Instance.SwitchWeapon(-1); // �л�����һ������
            }
            else if(Input.GetKeyDown(KeyCode.E))
            {
                _PlayerWeaponSystem.Instance.SwitchWeapon(1); // �л�����һ������
            }
        }
    }

    private void FixedUpdate()
    {
        if (IsAlive)
        {
            fsm.OnFixedUpdate(); // ����״̬��������״̬
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