using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class State_HumanEnemy_Idle : State
{
    FSM_Manager fsm;
    HumanEnemy self;
    public State_HumanEnemy_Idle(FSM_Manager fsmManager)
    {
        fsm = fsmManager;
        self = fsm.self as HumanEnemy;
    }
    public void OnEnter()
    {
        fsm.self.anim.SetBool("isIdle", true);
    }

    public void OnExit()
    {
        fsm.self.anim.SetBool("isIdle", false);
    }

    public void OnFixedUpdate()
    {

    }

    public void OnUpdate()
    {
        if (self.SearchForTarget())
        {
            fsm.SwitchState(StateType.Approch);
        }
        else if (Time.time - self.wanderTimer >= self.wanderCD)
        {
            fsm.SwitchState(StateType.Wander);
        }
    }


}

public class State_HumanEnemy_Wander : State
{
    FSM_Manager fsm;
    HumanEnemy self;
    Vector2 wanderPosition;
    public State_HumanEnemy_Wander(FSM_Manager fsmManager)
    {
        fsm = fsmManager;
        self = fsm.self as HumanEnemy;
    }

    public void OnEnter()
    {
        fsm.self.anim.SetBool("isMoving", true);
        self.wanderTimer = Time.time;
        wanderPosition = (Vector2)self.transform.position + Random.insideUnitCircle * self.wanderRadius;
    }

    public void OnExit()
    {
        fsm.self.anim.SetBool("isMoving", false);
    }

    public void OnFixedUpdate()
    {

    }

    public void OnUpdate()
    {
        if(self.SearchForTarget())
        {
            fsm.SwitchState(StateType.Approch);
        }
        else if (Vector2.Distance(self.transform.position,wanderPosition)>=0.1f)
        {
            self.rigid.velocity = (wanderPosition - (Vector2)self.transform.position).normalized * self.Speed;
        }
        else
        {
            fsm.SwitchState(StateType.Idle);
        }
    }
}

public class State_HumanEnemy_Approch : State
{
    FSM_Manager fsm;
    HumanEnemy self;

    public State_HumanEnemy_Approch(FSM_Manager fsmManager)
    {
        fsm = fsmManager;
        self = fsm.self as HumanEnemy;
    }
    public void OnEnter()
    {
        fsm.self.anim.SetBool("isMoving", true);
    }

    public void OnExit()
    {
        fsm.self.anim.SetBool("isMoving", false);
    }

    public void OnFixedUpdate()
    {

    }

    public void OnUpdate()
    {

        if(!self.SearchForTarget())
        {
            fsm.SwitchState(StateType.Idle);
        }
        else
        {
            if (self.target != null)
            {
                if (Vector2.Distance(self.target.transform.position, self.transform.position) <= 3f)
                {
                    self.Attack();
                }
            }
            self.rigid.velocity = (self.target.transform.position - self.transform.position).normalized * self.Speed;
        }
    }
}

public class State_HumanEnemy_Dead : State
{
    FSM_Manager fsm;
    public void OnEnter()
    {

    }

    public void OnExit()
    {

    }

    public void OnFixedUpdate()
    {

    }

    public void OnUpdate()
    {

    }


}

public class State_HumanEnemy_Damaged : State
{
    FSM_Manager fsm;
    bool animePlayed = false; // 用于标记是否正在播放受伤动画
    public State_HumanEnemy_Damaged(FSM_Manager fsmManager)
    {
        fsm = fsmManager;
    }

    public void OnEnter()
    {
        //Debug.Log("Damaged State Entered");
        fsm.self.anim.SetTrigger("Damaged");
        fsm.self.rigid.velocity = Vector2.zero; // Stop movement on damage
        animePlayed = false;

    }
    public void OnExit()
    {
    }
    public void OnFixedUpdate()
    {
        
    }
    public void OnUpdate()
    {
        if (fsm.self.anim.GetCurrentAnimatorStateInfo(0).IsName("Damaged"))
        {
            animePlayed = true; // 标记动画正在播放
        }
        else if (animePlayed)
        {
            // 动画播放完毕，切换回Idle状态
            fsm.SwitchState(StateType.Idle);
            animePlayed = false; // 重置标记
            //
        }
    }
}