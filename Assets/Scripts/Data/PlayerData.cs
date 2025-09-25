using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
/// <summary>
/// 玩家存档数据类，包含玩家所持物品与武器信息
/// </summary>
[CreateAssetMenu(fileName = "PlayerData", menuName = "ScriptableObjects/PlayerData", order = 1)]
public class PlayerData : ScriptableObject
{
    public int gold;
    public bool isGearRefreshed = false; // 标记物品是否已经刷新
    public List<_GameItem> playerItems;
    public List<_RuntimeGameItem> runtimeItems;
    public List<_GameWeaponItem> playerWeapons;
    public List<_RuntimeGameWeaponItem> runtimeWeapons;

}

public enum ItemType
{
    Weapon,
    Accessory,
    Consumable,
    Material
}
/// <summary>
/// 游戏物品基类，包含物品的基本属性与方法
/// </summary>
[CreateAssetMenu(fileName = "GameItem", menuName = "ScriptableObjects/GameItem", order = 2)]
public class _GameItem : ScriptableObject
{
    public string itemName;
    public Sprite itemIcon;
    public int itemValue;
    public int itemID;
    public ItemType itemType;
    public GameObject itemPrefab;

    public _RuntimeGameItem GetRuntimeItem()
    {
        return new _RuntimeGameItem(this);
    }
}
/// <summary>
/// 运行时游戏物品类，用于在游戏运行时管理物品实例，避免直接修改ScriptableObject数据
/// </summary>
public class  _RuntimeGameItem
{
    public string itemName;
    public Sprite itemIcon;
    public int itemValue;
    public int itemID;
    public ItemType itemType;
    public GameObject itemPrefab;
    
    public _RuntimeGameItem(_GameItem gameItem)
    {
        itemName = gameItem.itemName;
        itemIcon = gameItem.itemIcon;
        itemValue = gameItem.itemValue;
        itemID = gameItem.itemID;
        itemType = gameItem.itemType;
        itemPrefab = gameItem.itemPrefab;
    }
}

/// <summary>
/// 运行时玩家武器物品类，继承自运行时游戏物品类，包含武器实例化方法
/// 避免直接修改ScriptableObject数据
/// </summary>
public class _RuntimeGameWeaponItem 
{
    public WeaponAttribute weaponAttribute;
    public Vector2 _EquipementOffset;
    public GameObject itemPrefab;
    public _RuntimeGameWeaponItem(_GameWeaponItem gameWeaponItem) 
    {
        weaponAttribute = gameWeaponItem.weaponAttribute;
        itemPrefab = gameWeaponItem.itemPrefab;
        _EquipementOffset = gameWeaponItem._EquipementOffset;
    }
    public _RuntimeGameWeaponItem() { }

    /*    public void InstantiateWeapon(_GameEntity owner, Transform parent)
        {
            if (itemPrefab != null)
            {
                WeaponSystem.Instance.GenerateWeapon(this, owner);
            }
            else
            {
                Debug.LogError("Item prefab is null.");
            }
        }*/

}