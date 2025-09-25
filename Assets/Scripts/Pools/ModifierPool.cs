using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ������д�ˣ����������������Ĺ�����
/// 1.StoreManager��WeaponPool��������
/// 2.WeaponPool�����Լ��������ط���һ������
/// 3.StoreManager��AffixPool��������б�
/// 4.AffixPool����ϡ�жȷ���һ�������б�
/// 5.StoreManager��ModifierPool��������б�Ҳ��������ű���
/// 6.ModifierPool�����Լ��Ĵ����ط���һ�������б�
/// 7.��û����StoreManager��SpawnPoint������һ��StoreItemԤ���壬���������������б����η��б�ֵ����
/// 8.��ҹ�������ʱ��Interactable��OnInteract������������������ұ���
/// </summary>
public class ModifierPool : MonoBehaviour
{
    public static ModifierPool Instance;
    public List<Type> modifiers;
    private static List<Type> modifierPool = new List<Type>
    {
        typeof(M_Berserker),
        // ...�Ժ���������չ
    };

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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    internal List<WeaponModifier> RequestModifiers()
    {
        throw new NotImplementedException();
    }
}
