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
/// buuff���࣬����buff����Ҫ�̳��Դ��࣬һ����˵����һ��buff��ķ�����
/// 1.����һ���̳���Buff����
/// 2.ʵ��OnTick,OnBuffStart,OnBuffEnd����
/// 3.ʵ�ֲ��ڹ��캯���е���BuffInit���г�ʼ����һ�㹹�캯����Ҫһ��duration����
/// </summary>
public abstract class Buff
{
    public BuffType type;   //����Ҫ buff�����ͣ�BuffManagerɾ��buffʱ���ݴ�����ɾ��
    public float Duration; // Duration in seconds
    public bool IsPositive; // True for buff, false for debuff
    public abstract void OnTick(_GameEntity entity); // Called periodically
    public abstract void OnBuffEnd(_GameEntity entity); // Called when buff ends
    public abstract void OnBuffStart(_GameEntity entity); // Called when buff start
    /// <summary>
    /// ����buff����Ҫ���ô˺������г�ʼ��
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

