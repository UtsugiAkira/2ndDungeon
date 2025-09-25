using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EntityEventType
{
    None,
    OnHealthChanged,
    OnSpawn,
    OnInjury,
    OnHeal,
    OnAttack,
    OnHit,
    OnKill,
    OnMove,
    OnBuffTick,
}

public class EntityEventCenter :MonoBehaviour
{
    private Dictionary<EntityEventType, System.Action> eventDictionary ;

    private void Awake()
    {
        eventDictionary = new Dictionary<EntityEventType, System.Action>();
    }

    // Start is called before the first frame update

    public void AddEvent(EntityEventType type, System.Action method)
    {
        Debug.Log(eventDictionary);
        if (eventDictionary.ContainsKey(type))
        {
            eventDictionary[type] += method;
        }
        else
        {
            eventDictionary.Add(type, method);
        }
    }

    public void RemoveEvent(EntityEventType type, System.Action method)
    {
        if (eventDictionary.ContainsKey(type))
        {
            eventDictionary[type] -= method;
            if (eventDictionary[type] == null)
            {
                eventDictionary.Remove(type);
            }
        }
    }

    public void TriggerEvent(EntityEventType type)
    {
        if (eventDictionary.ContainsKey(type))
        {
            eventDictionary[type]?.Invoke();
        }
    }

    public void ClearEvents()
    {
        eventDictionary.Clear();
    }

}

