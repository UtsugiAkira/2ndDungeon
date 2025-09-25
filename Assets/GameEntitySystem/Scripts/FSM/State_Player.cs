using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class  State_Player_Idle:State
{
    private FSM_Manager fsmManager;
    public State_Player_Idle(FSM_Manager fsmManager)
    {
        this.fsmManager = fsmManager;
    }
    public void OnEnter()
    {
        // Set animation to idle
        fsmManager.self.anim.SetBool("isIdle", true);
    }
    public void OnUpdate()
    {
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S))
        {
            fsmManager.SwitchState(StateType.Move);
        }
    }
    public void OnExit()
    {
        // Reset animation state
        fsmManager.self.anim.SetBool("isIdle", false);
    }
    public void OnFixedUpdate()
    {
        // Handle physics-related updates if necessary
    }
}

public class State_Player_Move : State
{
    private FSM_Manager fsmManager;
    public State_Player_Move(FSM_Manager fsmManager)
    {
        this.fsmManager = fsmManager;
    }
    
    public void OnEnter()
    {
        // Set animation to move
        fsmManager.self.anim.SetBool("isMoving", true);
    }
    
    public void OnUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector2 movement = new Vector2(moveHorizontal, moveVertical);
        if (moveHorizontal > 0 && fsmManager.self.transform.localScale.x < 0)
        {
            fsmManager.self.transform.localScale = new Vector3(1, 1, 1);
        }
        else if (moveHorizontal < 0 && fsmManager.self.transform.localScale.x > 0)
        {
            fsmManager.self.transform.localScale = new Vector3(-1, 1, 1);
        }
        if (movement.magnitude == 0)
        {
            fsmManager.SwitchState(StateType.Idle);
        }
        fsmManager.self.rigid.velocity = movement * fsmManager.self.Speed;
    }
    
    public void OnExit()
    {
        // Reset animation state
        fsmManager.self.anim.SetBool("isMoving", false);
    }
    
    public void OnFixedUpdate()
    {
        // Handle physics-related updates if necessary
    }

    public void moving()
    {
        
    }


}

public class State_player_Damaged : State
{
    private FSM_Manager fsmManager;
    public State_player_Damaged(FSM_Manager fsmManager)
    {
        this.fsmManager = fsmManager;
    }
    
    public void OnEnter()
    {
        // Set animation to damaged
        fsmManager.self.anim.SetTrigger("isDamaged");
    }
    
    public void OnUpdate()
    {
        // Handle logic for being damaged, e.g., wait for a few seconds before returning to idle
        if (Time.time - fsmManager.tempTimer > 1.0f) // Assuming 1 second of damage state
        {
            fsmManager.SwitchState(StateType.Idle);
        }
    }
    
    public void OnExit()
    {
        // Reset animation state
    }
    
    public void OnFixedUpdate()
    {
        // Handle physics-related updates if necessary
    }
}

public class State_Player_Dead : State
{
    private FSM_Manager fsmManager;
    public State_Player_Dead(FSM_Manager fsmManager)
    {
        this.fsmManager = fsmManager;
    }

    public void OnEnter()
    {
        // Set animation to damaged
        fsmManager.self.anim.SetBool("isDead",true);
    }

    public void OnUpdate()
    {
    }

    public void OnExit()
    {
        fsmManager.self.anim.SetBool("isDead", true);
    }

    public void OnFixedUpdate()
    {
        // Handle physics-related updates if necessary
    }
}


#region Special States
///这个Region里的状态是专门面向本次项目的，不具备通用性。未来在其他项目中使用这个脚本时可以根据需求删除这些状态

/// <summary>
/// 特殊状态：实体冻结，用于处理诸如骑乘等特殊的需要限制实体行动并赋予无敌的状态
///</summary>
public class State_Special_EntityFreeze : State
{
    private FSM_Manager fsmManager;
    public State_Special_EntityFreeze(FSM_Manager fsmManager)
    {
        this.fsmManager = fsmManager;
    }
    public void OnEnter()
    {
        // Set animation to damaged
        fsmManager.self.anim.SetBool("isIdle", true);
        BuffManager.instance.AddBuff(fsmManager.self, new UnHarmable(-1));
    }
    public void OnUpdate()
    {
    }
    public void OnExit()
    {
        BuffManager.instance.RemoveBuff(fsmManager.self, BuffType.UnHarmable);
    }
    public void OnFixedUpdate()
    {
        // Handle physics-related updates if necessary
    }
}

#endregion