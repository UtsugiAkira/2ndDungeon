using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _DebugMenu : MonoBehaviour
{
    [SerializeField]private string buffTarget = "BuffName";
    private void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 300, 20), "Debug");
        buffTarget = GUI.TextField(new Rect(10, 25, 100, 20),buffTarget);
        if (GUI.Button(new Rect(10, 40, 100, 30), "Buff active"))
        {
            if(Enum.TryParse<BuffType>(buffTarget, out BuffType buffType))
            {
                switch (buffType)
                {
                    case BuffType.None:
                        break;
                    case BuffType.Guarding:
                        BuffManager.instance.AddBuff(_GamePlayer.Instance, new Guarding(10));
                        break;
                    case BuffType.Strength:
                        break;
                    case BuffType.Agility:
                        break;
                    case BuffType.Intelligence:
                        break;
                    case BuffType.Poison:
                        BuffManager.instance.AddBuff(_GamePlayer.Instance, new Poision(10,2));
                        break;
                    case BuffType.Regeneration:
                        break;
                    case BuffType.Shield:
                        break;
                    case BuffType.SpeedBoost:
                        break;
                    case BuffType.Slow:
                        break;
                    case BuffType.Stun:
                        break;
                    case BuffType.Healing:
                        BuffManager.instance.AddBuff(_GamePlayer.Instance, new Healing(10,2));
                        break;
                    case BuffType.Pushback:
                        BuffManager.instance.AddBuff(_GamePlayer.Instance, new Pushback(Vector2.left,10));

                        break;
                    case BuffType.Weakness:
                        BuffManager.instance.AddBuff(_GamePlayer.Instance, new Weakness(10,0.5f));

                        break;
                    default:
                        break;
                }
            }
        }
    }
}
