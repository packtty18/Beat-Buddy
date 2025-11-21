using System;
using System.Collections.Generic;
using UnityEngine;


//씬에 존재하는 모든 팩토리를 관리
public class PoolManager : SimpleSingleton<PoolManager>
{
    [Header("Pools")]
    [SerializeField] private PoolBase[] _poolList;
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
                _poolMap.Add(type, pool);
            }
        }
    }

    public T GetPool<T>() where T : PoolBase
    {
        Type key = typeof(T);
        if (_poolMap.TryGetValue(key, out PoolBase value))
        {
            return value as T;
        }

        Debug.LogError($"PoolManager: {key.Name} is not in the Scene!");
        return null;
    }

}
