using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreManager : MonoBehaviour
{
    public static StoreManager Instance;
    [SerializeField] private GameObject storeItemPrefab;
    [SerializeField] private GameObject itemSpawnParent;
    [SerializeField] private List<Transform> itemSpawnPoints;

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
        itemSpawnPoints = new List<Transform>();
        foreach (Transform child in itemSpawnParent.transform)
        {
            itemSpawnPoints.Add(child);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerDataManager.Instance.playerData.isGearRefreshed)
        {
            return;
        }
        else
        {
            RefreshGear();
            PlayerDataManager.Instance.playerData.isGearRefreshed = true;
        }
    }
    // Update is called once per frame
    void Update()
    {

    }


    public void RefreshGear()
    {
        
        Vector2 spawnPoint = itemSpawnPoints[Random.Range(0, itemSpawnPoints.Count)].position;
        StoreItem storeItem = new StoreItem();
        storeItem.item = WeaponPool.Instance.RequestRandomWeapon();
        storeItem.goldCost = SetRandomGoldCost();
        storeItem.item.weaponAttribute.affixes = AffixPool.Instance.RequestAffixes(AffixPool.GetRandomEnumValue<Rarity>());
        storeItem.item.weaponAttribute.modifiers = ModifierPool.Instance.RequestModifiers();
        //后面还差生成预制体的部分
    }

    public int SetRandomGoldCost()
    {
        return Random.Range(20, 51);

    }
}
