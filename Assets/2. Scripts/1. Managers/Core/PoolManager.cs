using System;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : CoreSingleton<PoolManager>
{
    [Header("Pools")]
    private PoolBase[] _poolList;
    private Dictionary<Type, PoolBase> _poolMap;


    protected override void Awake()
    {
        base.Awake();
        SetPoolMap();
    }

    public void SetPoolMap()
    {
        _poolMap = new Dictionary<Type, PoolBase>();
        _poolList = FindObjectsByType<PoolBase>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        foreach (PoolBase pool in _poolList)
        {
            if (pool == null)
            {
                continue;
            }
            Type type = pool.GetType();
            if (!_poolMap.ContainsKey(type))
            {
                pool.InitPool();
                _poolMap.Add(type, pool);
            }
        }
    }

    private T GetPool<T>() where T : PoolBase
    {
        Type key = typeof(T);
        if (_poolMap.TryGetValue(key, out PoolBase value))
        {
            return value as T;
        }

        Debug.LogError($"PoolManager: {key.Name} is not in the Scene!");
        return null;
    }

    public TComponent SpawnGetComponent<TPool,TEnum,TComponent>(TEnum type)
        where TPool : PoolBase<TEnum>
        where TEnum : Enum
        where TComponent : Component
    {
        GameObject obj = Spawn<TPool, TEnum>(type);
        if (obj == null)
        {
            Debug.LogError("[PoolManager] Spawned object is null.");
            return null;
        }
        TComponent component = obj.GetComponent<TComponent>();
        if (component == null)
        {
            Despawn<TPool, TEnum>(type, obj);
            Debug.LogError($"[PoolManager] Component of Type {typeof(TComponent).Name} not found on spawned object.");
            return null;
        }
        return component;
    }

    public GameObject Spawn<TPool, TEnum>(TEnum type)
        where TPool : PoolBase<TEnum>
        where TEnum : Enum
    {
        TPool pool = GetPool<TPool>();
        if (pool == null)
        {
            Debug.LogError($"PoolManager: {typeof(TPool).Name} not found in scene!");
            return null;
        }

        return pool.GetObject(type);
    }

    public void Despawn<TPool, TEnum>(TEnum type, GameObject obj)
        where TPool : PoolBase<TEnum>
        where TEnum : Enum
    {
        if (obj == null)
        {
            Debug.LogWarning("[PoolManager] Object is null, cannot despawn.");
            return;
        }

        TPool pool = GetPool<TPool>();
        if (pool == null)
        {
            Debug.LogError($"PoolManager: {typeof(TPool).Name} not found in scene!");
            return;
        }

        pool.ReturnObject(type, obj);
    }
}
