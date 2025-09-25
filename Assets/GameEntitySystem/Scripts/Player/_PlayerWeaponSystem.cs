using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _PlayerWeaponSystem : MonoBehaviour
{
    [SerializeField] private int _MaxEquipmentCount;
    [SerializeField] private int _CurrentEquipmentIndex = 0;
    [SerializeField] private int _MaxInventoryItemCount;
    [InspectorName("Debug Weapon List")]public List<_GameWeaponItem> _Debug_GameWeaponItems;
    public List<_RuntimeGameWeaponItem> weaponInventory = new List<_RuntimeGameWeaponItem>();
    public static _PlayerWeaponSystem Instance;
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

    private void Start()
    {
        #region Debug Add Weapons
        weaponInventory = new List<_RuntimeGameWeaponItem>();
        if (_Debug_GameWeaponItems.Count > 0)
        {
            foreach (var item in _Debug_GameWeaponItems)
            {
                weaponInventory.Add(item.GetRuntimeWeaponItem());
            }
        }
        #endregion
    }

    public void AddWeapon(_RuntimeGameWeaponItem newWeapon)
    {
        if (weaponInventory.Count >= _MaxEquipmentCount)
        {
            Debug.Log("Weapon inventory is full!");
            return;
        }
        weaponInventory.Add(newWeapon);
        Debug.Log($"Added {newWeapon.weaponAttribute.weaponName} to inventory.");
    }

    
    public void RemoveWeapon(_RuntimeGameWeaponItem weaponToRemove)
    {
        if (weaponInventory.Contains(weaponToRemove))
        {
            weaponInventory.Remove(weaponToRemove);
            Debug.Log($"Removed {weaponToRemove.weaponAttribute.weaponName} from inventory.");
        }
        else
        {
            Debug.Log("Weapon not found in inventory.");
        }
    }

    public void SwitchWeapon(int deltaindex)
    {
        if(_GamePlayer.Instance.currentWeapon?.CanAttack() == false)
        {
            Debug.Log("Cannot switch weapon while attacking.");
            return;
        }
        if (weaponInventory.Count == 0)
        {
            Debug.Log("No weapons in inventory to switch.");
            return;
        }
        do
        {
            _CurrentEquipmentIndex += deltaindex;
            if (_CurrentEquipmentIndex >= weaponInventory.Count)
            {
                _CurrentEquipmentIndex = 0;
            }
            else if (_CurrentEquipmentIndex < 0)
            {
                _CurrentEquipmentIndex = weaponInventory.Count - 1;
            }
        } while (weaponInventory[_CurrentEquipmentIndex] == null);
        Debug.Log($"Switched to {weaponInventory[_CurrentEquipmentIndex].weaponAttribute.weaponName}.");
        if (weaponInventory[_CurrentEquipmentIndex] == null)
        {
            Debug.Log("No weapon in the selected slot.");
            return;
        }
        WeaponSystem.Instance.GenerateWeapon(weaponInventory[_CurrentEquipmentIndex], _GamePlayer.Instance);
        //weaponInventory[_CurrentEquipmentIndex].InstantiateWeapon(_GamePlayer.Instance, _GamePlayer.Instance.transform);
    }

    public bool AddItem(_RuntimeGameWeaponItem newItem)
    {
        if (weaponInventory.Count < _MaxInventoryItemCount)
        {
            weaponInventory.Add(newItem);
            return true;
        }
        else
        {
            Debug.Log("Weapon inventory is full!");
            return false;
        }
    }
}
