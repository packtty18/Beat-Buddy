using System;
using System.Collections.Generic;
using UnityEngine;

public class FactoryManager : SimpleSingleton<FactoryManager>
{
    [Header("Factories")]
    [SerializeField] private List<FactoryBase> _factoryList = new List<FactoryBase>();
    private Dictionary<Type, FactoryBase> _factoryMap;

    protected override void Awake()
    {
        base.Awake();
        SetFactoryMap();
    }

    private void SetFactoryMap()
    {
        _factoryMap = new Dictionary<Type, FactoryBase>();
        foreach (FactoryBase factory in _factoryList)
        {
            if (factory == null) 
                continue;

            Type t = factory.GetType();
            if (!_factoryMap.ContainsKey(t))
            {
                _factoryMap.Add(t, factory);
            }
        }
    }

    public T GetFactory<T>() where T : FactoryBase
    {
        Type key = typeof(T);
        if (_factoryMap.TryGetValue(key, out FactoryBase value))
            return value as T;

        Debug.LogError($"FactoryManager: {key.Name} not registered!");
        return null;
    }

}
