using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BuffType
{
    None,
    Guarding,
    Strength,
    Agility,
    Intelligence,
    Poison,
    Regeneration,
    Shield,
    SpeedBoost,
    Slow,
    Stun,
    Healing,
    Pushback,
    Weakness,
    UnHarmable,
}
/// <summary>
/// buuff基类，所有buff都需要继承自此类，一般来说配置一个buff类的方法：
/// 1.创建一个继承自Buff的类
/// 2.实现OnTick,OnBuffStart,OnBuffEnd方法
/// 3.实现并在构造函数中调用BuffInit进行初始化，一般构造函数需要一个duration参数
/// </summary>
public abstract class Buff
{
    public BuffType type;   //※重要 buff的类型，BuffManager删除buff时根据此类型删除
    public float Duration; // Duration in seconds
    public bool IsPositive; // True for buff, false for debuff
    public abstract void OnTick(_GameEntity entity); // Called periodically
    public abstract void OnBuffEnd(_GameEntity entity); // Called when buff ends
    public abstract void OnBuffStart(_GameEntity entity); // Called when buff start
    /// <summary>
    /// 所有buff都需要调用此函数进行初始化
    /// </summary>
    /// <param name="type"></param>
    /// <param name="duration"></param>
    /// <param name="isPositive"></param>
    public void BuffInit(BuffType type, float duration, bool isPositive)
    {
        this.type = type;
        this.Duration = duration;
        this.IsPositive = isPositive;
    }
}

