
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healing : Buff
{
    private float healAmount;

    public Healing(float duration, float healAmount) 
    {
        BuffInit(BuffType.Healing, duration, true);
        this.healAmount = healAmount;
    }

    public override void OnBuffEnd(_GameEntity entity)
    {
    }

    public override void OnBuffStart(_GameEntity entity)
    {
    }

    public override void OnTick(_GameEntity entity)
    {
        entity.Heal(healAmount);
    }
}
