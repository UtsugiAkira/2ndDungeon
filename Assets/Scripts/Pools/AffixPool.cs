using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 稀有度枚举，影响词条数量
/// </summary>
public enum Rarity
{
    Common,
    Rare,
    Epic,
    Legendary
}

public class AffixPool : MonoBehaviour
{
    public static AffixPool Instance;
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
    }
    // 词条池（注意：这里只存类型，不存实例）
    private static List<Type> affixPool = new List<Type>
    {
        typeof(A_Broken),
        typeof(A_Sharpness),
        // typeof(A_ShootingSpeed),
        // typeof(A_BulletSpeed),
        // typeof(A_SwordRange),
        // typeof(A_SwordSwingAngle),
        // ...以后在这里扩展
    };

    public static T GetRandomEnumValue<T>() where T : Enum
    {
        Array values = Enum.GetValues(typeof(T));
        int index = UnityEngine.Random.Range(0, values.Length);
        return (T)values.GetValue(index);
    }

    /// <summary>
    /// 生成词条列表
    /// </summary>
    public List<IWeaponAffix> RequestAffixes(Rarity rarity)
    {
        int minCount, maxCount;

        // 根据稀有度决定词条数量范围
        switch (rarity)
        {
            case Rarity.Common:
                minCount = 1; maxCount = 2;
                break;
            case Rarity.Rare:
                minCount = 2; maxCount = 3;
                break;
            case Rarity.Epic:
                minCount = 3; maxCount = 4;
                break;
            case Rarity.Legendary:
                minCount = 4; maxCount = 5;
                break;
            default:
                minCount = 1; maxCount = 2;
                break;
        }

        int count = UnityEngine.Random.Range(minCount, maxCount + 1);
        List<IWeaponAffix> result = new List<IWeaponAffix>();

        // 随机挑选词条
        List<Type> available = new List<Type>(affixPool); // 拷贝，避免重复取同一个

        for (int i = 0; i < count && available.Count > 0; i++)
        {
            int index = UnityEngine.Random.Range(0, available.Count);
            Type affixType = available[index];

            // 反射生成实例
            IWeaponAffix affix = Activator.CreateInstance(affixType) as IWeaponAffix;
            if (affix != null)
            {
                result.Add(affix);
            }

            // 移除已使用的词条，避免重复
            available.RemoveAt(index);
        }

        return result;
    }
}
