using UnityEngine;
using System.Collections.Generic;
using TMPro;

namespace DamageDisplay
{
    [System.Serializable]
    public class DamageColorSetting
    {
        public DamageModifierType modifierType;
        public Color color;
    }
}
/// <summary>
/// 伤害数值显示控制器（其实是管理器级别）AI写的
/// [Header("References")]中的引用需要在Inspector中赋值
/// damageTextPrefab为Assets中的预制体，damageTextContainer为场景中的一个Canvas，用于组织生成的伤害数字
/// 下面的设置我还没玩明白
/// </summary>
public class DamageTextController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject damageTextPrefab;
    [SerializeField] private Transform damageTextContainer;

    [Header("Settings")]
    [SerializeField] private int poolSize = 20;
    [SerializeField] private Vector2 randomOffset = new Vector2(0.5f, 0.5f);

    [Header("Default Colors")]
    [SerializeField] private Color normalDamageColor = Color.white;
    [SerializeField] private Color criticalDamageColor = Color.yellow;
    [SerializeField] private Color healColor = Color.green;
    [SerializeField] private Color shieldColor = Color.blue;
    public List<DamageDisplay.DamageColorSetting> damageColorSettings ;
    private Dictionary<DamageModifierType, Color> damageColorDict;

    private Queue<GameObject> damageTextPool = new Queue<GameObject>();
    private EventCenter eventCenter;

    void Start()
    {
        eventCenter = EventCenter.instance;
        if (eventCenter == null)
        {
            Debug.LogError("EventCenter not found in scene!");
            return;
        }
        // 初始化颜色字典
        damageColorDict = new Dictionary<DamageModifierType, Color>();
        foreach (var setting in damageColorSettings)
        {
            if (!damageColorDict.ContainsKey(setting.modifierType))
            {
                damageColorDict.Add(setting.modifierType, setting.color);
            }
        }
        // 初始化对象池
        InitializePool();

        // 订阅伤害事件
        eventCenter.AddEvent<DamageData>(EventType.EntityDamage, OnCharacterDamaged);
    }

    private void InitializePool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject damageText = Instantiate(damageTextPrefab, damageTextContainer);
            damageText.SetActive(false);
            damageTextPool.Enqueue(damageText);
        }
    }

    private void OnCharacterDamaged(DamageData damageData)
    {
        // 从对象池获取伤害数字
        GameObject damageText = GetDamageTextFromPool();
        if (damageText == null) return;

        // 设置伤害数字位置（角色位置 + 随机偏移）
        Vector3 spawnPosition = damageData.targetPosition;
        spawnPosition.x += Random.Range(-randomOffset.x, randomOffset.x);
        spawnPosition.y += Random.Range(-randomOffset.y, randomOffset.y);
        damageText.transform.position = spawnPosition;

        // 设置伤害数字内容和样式
        DamageTextItem damageTextItem = damageText.GetComponent<DamageTextItem>();
        if (damageTextItem != null)
        {
            Color textColor = normalDamageColor;
            if (damageData.appliedModifiers.Count != 0)
            {
                textColor = damageColorDict.ContainsKey(damageData.appliedModifiers[0]) ? damageColorDict[damageData.appliedModifiers[0]] : normalDamageColor;
            }
            //Color textColor = damageColorDict.ContainsKey(damageData.appliedModifiers[0]) ? damageColorDict[damageData.appliedModifiers[0]] : normalDamageColor;
            // 确定颜色
            /*Color textColor = normalDamageColor;
            if (damageData.isCritical) textColor = criticalDamageColor;
            if (damageData.isHeal) textColor = healColor;*/
            // 初始化伤害数字
            damageTextItem.Initialize(damageData.amount, textColor, damageData.isCritical);
        }
        // 激活伤害数字
        damageText.SetActive(true);

    }

    private GameObject GetDamageTextFromPool()
    {
        if (damageTextPool.Count > 0)
        {
            return damageTextPool.Dequeue();
        }

        // 如果池子空了，创建一个新的
        GameObject newDamageText = Instantiate(damageTextPrefab, damageTextContainer);
        return newDamageText;
    }

    public void ReturnDamageTextToPool(GameObject damageText)
    {
        damageText.SetActive(false);
        damageTextPool.Enqueue(damageText);
    }

    void OnDestroy()
    {
        // 取消订阅事件
        if (eventCenter != null)
        {
            eventCenter.RemoveEvent<DamageData>(EventType.EntityDamage, OnCharacterDamaged);
        }
    }
}