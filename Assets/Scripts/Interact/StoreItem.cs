using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreItem : InteractableObject
{
    public _RuntimeGameWeaponItem item;
    public int goldCost;

    public override void OnInteractable()
    {
        if (PlayerDataManager.Instance.playerData.gold >= goldCost)
        {
            
            if (_PlayerWeaponSystem.Instance.AddItem(item))
            {
                PlayerDataManager.Instance.playerData.gold -= goldCost;
                Destroy(this.gameObject);
            }
            
        }
        else
        {
            Debug.Log("Not enough gold!");
        }
        
    }


}
