using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPool : MonoBehaviour
{
    public static WeaponPool Instance;
    [SerializeField] private List<_GameWeaponItem> weaponItems;
    private List<_RuntimeGameWeaponItem> runtimeWeaponItems;    

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        runtimeWeaponItems = new List<_RuntimeGameWeaponItem>();
        foreach (var item in weaponItems)
        {
            runtimeWeaponItems.Add(item.GetRuntimeWeaponItem());
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public _RuntimeGameWeaponItem RequestRandomWeapon()
    {
        int index = Random.Range(0, runtimeWeaponItems.Count);
        _RuntimeGameWeaponItem weaponItem = runtimeWeaponItems[index];
        return weaponItem;
    }
}
