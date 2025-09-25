using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnHarmable : Buff
{
    DM_UnHarmable modifier = new DM_UnHarmable();
    public UnHarmable(float duration)
    {
        this.Duration = duration;
        BuffInit(BuffType.UnHarmable, duration, true);
    }
    public override void OnBuffEnd(_GameEntity entity)
    {
        Debug.Log("Entity is no longer UnHarmable");
        entity.damageModifiers.Remove(modifier);
    }

    public override void OnBuffStart(_GameEntity entity)
    {
        Debug.Log("Entity is now UnHarmable");
        entity.damageModifiers.Add(modifier);
    }

    public override void OnTick(_GameEntity entity)
    {
        Debug.Log("UnHarmable buff ticking");
    }
}
