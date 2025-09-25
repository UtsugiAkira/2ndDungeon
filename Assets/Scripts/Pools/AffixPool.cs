using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ϡ�ж�ö�٣�Ӱ���������
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
    // �����أ�ע�⣺����ֻ�����ͣ�����ʵ����
    private static List<Type> affixPool = new List<Type>
    {
        typeof(A_Broken),
        typeof(A_Sharpness),
        // typeof(A_ShootingSpeed),
        // typeof(A_BulletSpeed),
        // typeof(A_SwordRange),
        // typeof(A_SwordSwingAngle),
        // ...�Ժ���������չ
    };

    public static T GetRandomEnumValue<T>() where T : Enum
    {
        Array values = Enum.GetValues(typeof(T));
        int index = UnityEngine.Random.Range(0, values.Length);
        return (T)values.GetValue(index);
    }

    /// <summary>
    /// ���ɴ����б�
    /// </summary>
    public List<IWeaponAffix> RequestAffixes(Rarity rarity)
    {
        int minCount, maxCount;

        // ����ϡ�жȾ�������������Χ
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

        // �����ѡ����
        List<Type> available = new List<Type>(affixPool); // �����������ظ�ȡͬһ��

        for (int i = 0; i < count && available.Count > 0; i++)
        {
            int index = UnityEngine.Random.Range(0, available.Count);
            Type affixType = available[index];

            // ��������ʵ��
            IWeaponAffix affix = Activator.CreateInstance(affixType) as IWeaponAffix;
            if (affix != null)
            {
                result.Add(affix);
            }

            // �Ƴ���ʹ�õĴ����������ظ�
            available.RemoveAt(index);
        }

        return result;
    }
}
