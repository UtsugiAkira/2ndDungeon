using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

/// <summary>
/// 该代码定义并实现了供游戏对象使用的有限状态机管理器（第一次手写有限状态机太刺激辣）
/// 麻了手关节好疼，感觉快得腱鞘炎了晚上回来再写罢
/// </summary>

public enum StateType
{
    Idle,
    Move,
    Jump,
    Attack,
    UsingItem,
    Damaged,
    Pushback,
    Dead,
    Fall,
    Approch,
    Wander,
    Freeze,
}

public class FSM_Manager
{
    [SerializeField]
    private State currentState;
    public StateType currentType;
    private Dictionary<StateType, State> stateList;
    public _GameEntity self;                                                        //供状态访问的GameEnemy属性
    public _GamePlayer player;
    public float tempTimer;
    public StateType lastState;
    public bool switchingState = false; // 用于标记是否正在切换状态

    public FSM_Manager(_GameEntity self)
    {
        stateList = new Dictionary<StateType, State>();
        this.self = self;
    }

    public void AddState(StateType type,State state)
    {
        if (stateList.ContainsKey(type))
        {
            Debug.Log(type + "Already have state");
            return;
        }
        else
        {
            stateList.Add(type, state);
        }
    }

    public void SwitchState(StateType target)
    {
        switchingState = true; // 设置状态切换标志为 true
        //Debug.Log("Switch>>>>>From:" + currentType.ToString() + ">>>>>To:" + target.ToString());
        if (!stateList.ContainsKey(target))
        {
            Debug.Log(self.transform.name + "Do not have state:" + target.ToString());
            return;
        }
        if(currentState != null)
        {
            currentState.OnExit();
        }
        currentType = target;
        currentState = stateList[target];
        currentState.OnEnter();
        switchingState = false; // 状态切换完成，重置标志
    }

    public void OnFixedUpdate()
    {
        if(switchingState) return; // 如果正在切换状态，则不执行当前状态的 FixedUpdate
        currentState.OnFixedUpdate();
    }

    public void OnUpdate()
    {
        if(switchingState) return; // 如果正在切换状态，则不执行当前状态的 Update
        currentState.OnUpdate();
    }
   
    public void ForceCrossFade( Animator animator, string name, float transitionDuration, int layer = 0, float normalizedTime = float.NegativeInfinity)
    {
        animator.Update(0);

        if (animator.GetNextAnimatorStateInfo(layer).fullPathHash == 0)
        {
            animator.CrossFade(name, transitionDuration, layer, normalizedTime);
        }
        else
        {
            animator.Play(animator.GetNextAnimatorStateInfo(layer).fullPathHash, layer);
            animator.Update(0);
            animator.CrossFade(name, transitionDuration, layer, normalizedTime);
        }
    }
    
    
}


