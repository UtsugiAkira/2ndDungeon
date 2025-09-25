using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poision : Buff
{
    public float tickDamage;

    public Poision(float duration, float tickDamage) 
    {
        BuffInit(BuffType.Poison,duration,false);
        this.tickDamage = tickDamage;
    }
    public override void OnBuffEnd(_GameEntity entity)
    {
    }

    public override void OnBuffStart(_GameEntity entity)
    {
    }

    public override void OnTick(_GameEntity entity)
    {
        entity.Damage(tickDamage,DamageType.Poison);
    }

}
