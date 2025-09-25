using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pushback : Buff
{
    private Vector2 pushDirection;
    private float pushDistance = 1f; // Push distance

    public Pushback(Vector2 direction, float distance)
    {
        BuffInit(BuffType.Pushback, 0f, false); // Instantaneous buff
        pushDirection = direction.normalized;
        pushDistance = distance;
    }

    public override void OnBuffEnd(_GameEntity entity)
    {
    }

    public override void OnBuffStart(_GameEntity entity)
    {
        entity.rigid.AddForce(pushDirection * pushDistance, ForceMode2D.Impulse);
    }

    public override void OnTick(_GameEntity entity)
    {
    }
}
