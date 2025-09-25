using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSystem : MonoBehaviour
{
    public static WeaponSystem Instance;
    public GameObject debugTarget;
    public GameObject debugEnemyWeapon;
    public GameObject debugEnemy;

    private void Start()
    {
        if (debugTarget != null)
        {
            WeaponSystem.ChangeWeapon(_GamePlayer.Instance, debugTarget.GetComponent<Weapon>());
        }
        if (debugEnemy != null && debugEnemyWeapon != null)
        {
            WeaponSystem.ChangeWeapon(debugEnemy.GetComponent<_GameEntity>(), debugEnemyWeapon.GetComponent<Weapon>());
        }
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static void ChangeWeapon(_GameEntity target, Weapon newWeapon)
    {
        if (target == null || newWeapon == null)
        {
            Debug.LogError($"Target or new weapon is null.Target:{target == null }|Weapon:{newWeapon == null}");
            return;
        }
        target.currentWeapon?.WeaponUninit();
        Destroy(target.currentWeapon?.gameObject);
        target.currentWeapon = newWeapon; // 更换武器
        target.currentWeapon.owner = target; // 设置武器的拥有者
    }

    public static void GenerateRandomSword(_GameEntity target)
    {

    }

    public static void GenerateRandomModifiers(Weapon weapon)
    {
        #region Debug
        weapon.WeaponAttribute.modifiers.Clear();
        weapon.WeaponAttribute.modifiers.Add(new M_Berserker());

        #endregion
    }

    public void GenerateWeapon(_RuntimeGameWeaponItem weapon,_GameEntity owner)
    {
        if (weapon.itemPrefab != null)
        {
            GameObject weaponObj = 
            Instantiate(weapon.itemPrefab, owner.transform.position, Quaternion.identity, owner.transform);
            weaponObj.transform.localPosition = weapon._EquipementOffset;
            Weapon weaponComponent = weaponObj.GetComponent<Weapon>();
            if(weaponComponent != null)
            {
                ChangeWeapon(owner, weaponComponent);
                weaponComponent.WeaponInit(weapon.weaponAttribute);
                return ;
            }
            else
            {
                Debug.LogError("The weapon prefab does not have a Weapon component.");
                Destroy(weaponObj);
                return ;
            }
        }
    }
}
