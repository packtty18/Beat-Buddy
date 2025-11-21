using System;
using System.Collections.Generic;
using UnityEngine;


//씬에 존재하는 모든 팩토리를 관리
public class PoolManager : SimpleSingleton<PoolManager>
{
    [Header("Pools")]
    private PoolBase[] _poolList;
    private Dictionary<Type, PoolBase> _poolMap;


    protected override void Awake()
    {
        base.Awake();
        SetPoolMap();
    }

    private void SetPoolMap()
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

    public GameObject Spawn<TPool, TEnum>(TEnum type)
        where TPool : PoolBase<TEnum>
        where TEnum : Enum
    {
        // Pool 가져오기
        TPool pool = GetPool<TPool>();
        if (pool == null)
        {
            Debug.LogError($"PoolManager: {typeof(TPool).Name} not found in scene!");
            return null;
        }

        // 풀에서 오브젝트 가져오기
        return pool.GetObject(type);
    }

    public void Despawn<TPool, TEnum>(TEnum type, GameObject obj)
        where TPool : PoolBase<TEnum>
        where TEnum : Enum
    {
        if (obj == null)
        {
            Debug.LogWarning("[PoolManager] Prefab is null, cannot despawn.");
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
