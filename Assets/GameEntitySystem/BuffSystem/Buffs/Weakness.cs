using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weakness : Buff
{
    private float Intencity; // 减少的攻击力百分比
    
    public Weakness(float duration, float intencity)
    {
        BuffInit(BuffType.Weakness, duration, false);
        Intencity = intencity;
    }

    public override void OnBuffEnd(_GameEntity entity)
    {
        entity.AttackDamageMuliplier /= (1 - Intencity);
    }

    public override void OnBuffStart(_GameEntity entity)
    {
        entity.AttackDamageMuliplier *= (1 - Intencity);
    }

    public override void OnTick(_GameEntity entity)
    {
    }
}
