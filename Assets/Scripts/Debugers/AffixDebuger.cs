using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AffixDebuger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        _GamePlayer player;
        Debug.Log("AffixDebuger Triggered");
        if (collision.TryGetComponent(out player))
        {
            List<IWeaponAffix> affixes = AffixPool.Instance.RequestAffixes(AffixPool.GetRandomEnumValue<Rarity>());
            _PlayerWeaponSystem.Instance.weaponInventory[0].weaponAttribute.SetAffixes(
             affixes);
            Debug.Log("Affixes Generated"+affixes);
        }
    }
}
