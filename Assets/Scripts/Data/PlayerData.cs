using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
/// <summary>
/// ��Ҵ浵�����࣬�������������Ʒ��������Ϣ
/// </summary>
[CreateAssetMenu(fileName = "PlayerData", menuName = "ScriptableObjects/PlayerData", order = 1)]
public class PlayerData : ScriptableObject
{
    public int gold;
    public bool isGearRefreshed = false; // �����Ʒ�Ƿ��Ѿ�ˢ��
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
/// ��Ϸ��Ʒ���࣬������Ʒ�Ļ��������뷽��
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
/// ����ʱ��Ϸ��Ʒ�࣬��������Ϸ����ʱ������Ʒʵ��������ֱ���޸�ScriptableObject����
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
/// ����ʱ���������Ʒ�࣬�̳�������ʱ��Ϸ��Ʒ�࣬��������ʵ��������
/// ����ֱ���޸�ScriptableObject����
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