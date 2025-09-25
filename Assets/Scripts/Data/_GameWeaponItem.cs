using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 玩家武器物品类，继承自游戏物品基类，包含武器特有属性与方法
/// 用于配置武器的基本Attribute与Prefab
/// </summary>
[CreateAssetMenu(fileName = "GameWeaponItem", menuName = "ScriptableObjects/GameWeaponItem", order = 3)]
public class _GameWeaponItem : ScriptableObject
{
    public WeaponAttribute weaponAttribute;
    public GameObject itemPrefab;
    public Vector2 _EquipementOffset;
    public _RuntimeGameWeaponItem GetRuntimeWeaponItem()
    {
        _RuntimeGameWeaponItem runtimeItem = new _RuntimeGameWeaponItem();
        runtimeItem.weaponAttribute = new WeaponAttribute( this.weaponAttribute.weaponName,this.weaponAttribute.weaponType,this.weaponAttribute.attackDamage);
        runtimeItem.itemPrefab = this.itemPrefab;
        runtimeItem._EquipementOffset = new Vector2( this._EquipementOffset.x,this._EquipementOffset.y);
        return runtimeItem;
    }
}
