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
        entity.speedMuliplier *= 2f; // �ָ�ԭʼ�ٶ�
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
        entity.speedMuliplier *= 0.5f; // �ٶȼ���
        entity.damageModifiers.Add(guardModifier);

    }

    public override void OnTick(_GameEntity entity)
    {
        entity.Mp -= 1f; // ÿ�����1��MP
        if (entity.Mp <= 0)
        {
            entity.Mp = 0;
            Duration = 0; // MP�ľ�����������״̬
        }
    }
}
