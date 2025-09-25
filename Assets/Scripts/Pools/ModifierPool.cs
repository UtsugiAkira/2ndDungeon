using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 明天再写了，理论上生成武器的过程是
/// 1.StoreManager从WeaponPool请求武器
/// 2.WeaponPool根据自己的武器池返回一个武器
/// 3.StoreManager从AffixPool请求词条列表
/// 4.AffixPool根据稀有度返回一个词条列表
/// 5.StoreManager从ModifierPool请求词条列表（也就是这个脚本）
/// 6.ModifierPool根据自己的词条池返回一个词条列表
/// 7.（没做）StoreManager在SpawnPoint处生成一个StoreItem预制体，并把武器、词条列表、修饰符列表赋值给它
/// 8.玩家购买武器时走Interactable的OnInteract方法，把武器加入玩家背包
/// </summary>
public class ModifierPool : MonoBehaviour
{
    public static ModifierPool Instance;
    public List<Type> modifiers;
    private static List<Type> modifierPool = new List<Type>
    {
        typeof(M_Berserker),
        // ...以后在这里扩展
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
