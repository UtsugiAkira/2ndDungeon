using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guarding : Buff
{
    private DM_Guard guardModifier;
    public Guarding(float duration)
    {
        guardModifier = new DM_Guard();
        BuffInit(BuffType.Guarding, duration, true);
    }

    public override void OnBuffEnd(_GameEntity entity)
    {
        entity.speedMuliplier *= 2f; // 恢复原始速度
        entity.damageModifiers.Remove(guardModifier);
    }

    public override void OnBuffStart(_GameEntity entity)
    {
        if (entity.Mp <= 0)
        {
            BuffManager.instance.RemoveBuff(entity, this.type);
            entity.damageModifiers.Remove(guardModifier);
            return;
        }
        entity.speedMuliplier *= 0.5f; // 速度减半
        entity.damageModifiers.Add(guardModifier);

    }

    public override void OnTick(_GameEntity entity)
    {
        entity.Mp -= 1f; // 每秒减少1点MP
        if (entity.Mp <= 0)
        {
            entity.Mp = 0;
            Duration = 0; // MP耗尽，结束防御状态
        }
    }
}
