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
/// �˺���ֵ��ʾ����������ʵ�ǹ���������AIд��
/// [Header("References")]�е�������Ҫ��Inspector�и�ֵ
/// damageTextPrefabΪAssets�е�Ԥ���壬damageTextContainerΪ�����е�һ��Canvas��������֯���ɵ��˺�����
/// ����������һ�û������
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
        // ��ʼ����ɫ�ֵ�
        damageColorDict = new Dictionary<DamageModifierType, Color>();
        foreach (var setting in damageColorSettings)
        {
            if (!damageColorDict.ContainsKey(setting.modifierType))
            {
                damageColorDict.Add(setting.modifierType, setting.color);
            }
        }
        // ��ʼ�������
        InitializePool();

        // �����˺��¼�
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
        // �Ӷ���ػ�ȡ�˺�����
        GameObject damageText = GetDamageTextFromPool();
        if (damageText == null) return;

        // �����˺�����λ�ã���ɫλ�� + ���ƫ�ƣ�
        Vector3 spawnPosition = damageData.targetPosition;
        spawnPosition.x += Random.Range(-randomOffset.x, randomOffset.x);
        spawnPosition.y += Random.Range(-randomOffset.y, randomOffset.y);
        damageText.transform.position = spawnPosition;

        // �����˺��������ݺ���ʽ
        DamageTextItem damageTextItem = damageText.GetComponent<DamageTextItem>();
        if (damageTextItem != null)
        {
            Color textColor = normalDamageColor;
            if (damageData.appliedModifiers.Count != 0)
            {
                textColor = damageColorDict.ContainsKey(damageData.appliedModifiers[0]) ? damageColorDict[damageData.appliedModifiers[0]] : normalDamageColor;
            }
            //Color textColor = damageColorDict.ContainsKey(damageData.appliedModifiers[0]) ? damageColorDict[damageData.appliedModifiers[0]] : normalDamageColor;
            // ȷ����ɫ
            /*Color textColor = normalDamageColor;
            if (damageData.isCritical) textColor = criticalDamageColor;
            if (damageData.isHeal) textColor = healColor;*/
            // ��ʼ���˺�����
            damageTextItem.Initialize(damageData.amount, textColor, damageData.isCritical);
        }
        // �����˺�����
        damageText.SetActive(true);

    }

    private GameObject GetDamageTextFromPool()
    {
        if (damageTextPool.Count > 0)
        {
            return damageTextPool.Dequeue();
        }

        // ������ӿ��ˣ�����һ���µ�
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
        // ȡ�������¼�
        if (eventCenter != null)
        {
            eventCenter.RemoveEvent<DamageData>(EventType.EntityDamage, OnCharacterDamaged);
        }
    }
}