using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EventType
{
    None,
    EntityDamage,
    PlayerDie,
    EnemyDie,
    GameWin,
    GameLose,
    PauseGame,
    ResumeGame,
    CollectItem,
    UseItem,
    OpenInventory,
    CloseInventory,
    LevelUp,
    SkillUnlock,
    QuestComplete,
    AchievementUnlocked
}

public class EventCenter : MonoBehaviour
{
    public static EventCenter instance;
    private Dictionary<EventType, System.Action> eventDictionary = new Dictionary<EventType, System.Action>();
    // 添加一个支持泛型参数的事件字典
    private Dictionary<EventType, System.Delegate> eventDictionaryWithParams =
        new Dictionary<EventType, System.Delegate>();
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
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

    public void AddEvent(EventType type,Action method)
    {
        if(eventDictionary.ContainsKey(type))
        {
            eventDictionary[type] += method;
        }
        else
        {
            eventDictionary.Add(type, method);
        }
    }

    public void RemoveEvent(EventType type, Action method)
    {
        if(eventDictionary.ContainsKey(type))
        {
            eventDictionary[type] -= method;
            if(eventDictionary[type] == null)
            {
                eventDictionary.Remove(type);
            }
        }
    }

    public void TriggerEvent(EventType type)
    {
        if(eventDictionary.ContainsKey(type))
        {
            eventDictionary[type]?.Invoke();
        }else if(eventDictionaryWithParams.ContainsKey(type))
        {
            eventDictionaryWithParams[type]?.DynamicInvoke();
        }
    }

    public void ClearEvents()
    {
        eventDictionary.Clear();
    }

    

    // 添加带参数的事件订阅方法
    public void AddEvent<T>(EventType type, System.Action<T> method)
    {
        if (eventDictionaryWithParams.ContainsKey(type))
        {
            eventDictionaryWithParams[type] = System.Delegate.Combine(
                eventDictionaryWithParams[type], method);
        }
        else
        {
            eventDictionaryWithParams.Add(type, method);
        }
    }

    public void TriggerEvent<T>(EventType type, T arg)
    {
        if (eventDictionaryWithParams.TryGetValue(type, out System.Delegate delegateObj))
        {
            System.Action<T> action = delegateObj as System.Action<T>;
            action?.Invoke(arg);
        }
    }

    // 添加对应的RemoveEvent泛型方法
    public void RemoveEvent<T>(EventType type, System.Action<T> method)
    {
        if (eventDictionaryWithParams.ContainsKey(type))
        {
            eventDictionaryWithParams[type] = System.Delegate.Remove(eventDictionaryWithParams[type], method);

            // 如果没有监听器了，移除键
            if (eventDictionaryWithParams[type] == null)
            {
                eventDictionaryWithParams.Remove(type);
            }
        }
    }
}
