using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDataNode 
{
    public string itemID; 
    public string itemName; 
    public string itemDescription; 
    public Sprite itemIcon; 
    public GameObject itemPrefab; 
}

public class WeaponDataNode 
{
    public WeaponAttribute weaponAttribute;
    public GameObject weaponPrefab;
}