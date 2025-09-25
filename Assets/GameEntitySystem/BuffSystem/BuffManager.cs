using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffManager : MonoBehaviour
{
    public static BuffManager instance;
    Dictionary<_GameEntity, Dictionary<BuffType,Buff>> entityBuffs ;


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

    private void Start()
    {
        entityBuffs = new Dictionary<_GameEntity, Dictionary<BuffType, Buff>>();
        StartCoroutine(BuffTick());
    }

    public IEnumerator BuffTick()
    {
        while (true)
        {
            foreach (var entityEntry in entityBuffs)
            {
                var entity = entityEntry.Key;
                var buffs = entityEntry.Value;
                List<BuffType> buffsToRemove = new List<BuffType>();
                foreach (var buffEntry in buffs)
                {
                    var buff = buffEntry.Value;
                    buff.OnTick(entity);
                    buff.Duration -= 1f; // Assuming tick interval is 1 second
                    if (buff.Duration == 0)
                    {
                        buffsToRemove.Add(buff.type);
                    }
                    //Debug.Log($"Buff: {buff.type}, Remaining Duration: {buff.Duration} on Entity: {entity.name}");
                }
                // Remove expired buffs
                foreach (var buffType in buffsToRemove)
                {
                    buffs.Remove(buffType);
                }
            }
            yield return new WaitForSeconds(1f); // Tick interval
        }
    }

    public void AddBuff(_GameEntity entity,Buff buff)
    {
        if (!entityBuffs.ContainsKey(entity) )
        {
            //Debug.Log("Adding new buff dictionary for entity: " + entity.name);
            entityBuffs.Add(entity, new Dictionary<BuffType, Buff>());
            entityBuffs[entity].Add(buff.type, buff);
            buff.OnBuffStart(entity);
            return;
        }
        if( !entityBuffs[entity].ContainsKey(buff.type))
        {
            //Debug.Log("Adding new buff: " + buff.type + " to entity: " + entity.name);
            buff.OnBuffStart(entity);
            entityBuffs[entity][buff.type] = buff;
        }
        else
        {
            //Debug.Log("Buff already exists. Refreshing duration for buff: " + buff.type + " on entity: " + entity.name);
            // If the buff already exists, refresh its duration or overlay effects
            entityBuffs[entity][buff.type].Duration = buff.Duration;

        }
        
    }

    public void RemoveBuff(_GameEntity entity,BuffType buffType)
    {
        if (entityBuffs.ContainsKey(entity) && entityBuffs[entity].ContainsKey(buffType))
        {
            entityBuffs[entity][buffType].OnBuffEnd(entity);
            entityBuffs[entity].Remove(buffType);
        }
    }

    public bool HasBuff(_GameEntity entity,BuffType buffType)
    {
        //Debug.Log($"result: has entity {entityBuffs.ContainsKey(entity)},has buff:{entityBuffs[entity].ContainsKey(buffType)}");
        return entityBuffs.ContainsKey(entity) && entityBuffs[entity].ContainsKey(buffType);
    }
}
