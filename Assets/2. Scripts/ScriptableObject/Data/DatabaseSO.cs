using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct BaseData<TKey, TValue>
{
    public TKey Key;
    public TValue Value;
}

public abstract class DatabaseSO<TKey, TValue> : ScriptableObject
{
    [SerializeField] protected List<BaseData<TKey, TValue>> entries = new List<BaseData<TKey, TValue>>();
    private Dictionary<TKey, TValue> map;

    public void InitMap()
    {
        map = new Dictionary<TKey, TValue>();
        foreach (var entry in entries)
        {
            if (!map.ContainsKey(entry.Key))
            {
                map.Add(entry.Key, entry.Value);
            }
            else
            {
                Debug.LogWarning($"Duplicate key detected: {entry.Key}");
            }
        }
    }

    public TValue GetData(TKey key)
    {
        if(map == null)
        {
            InitMap();
        }
        map.TryGetValue(key, out TValue value);
        return value;
    }

    public List<TValue> GetAllData()
    {
        List<TValue> values = new List<TValue>();
        foreach (var entry in entries)
        {
            if (entry.Value != null)
            {
                values.Add(entry.Value);
            }
        }
        return values;
    }

    public int Count => entries?.Count ?? 0;

    public bool ContainsKey(TKey key)
    {
        if (map == null)
        {
            InitMap();
        }
        return map.ContainsKey(key);
    }
}

