using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���������Ʒ�࣬�̳�����Ϸ��Ʒ���࣬�����������������뷽��
/// �������������Ļ���Attribute��Prefab
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
