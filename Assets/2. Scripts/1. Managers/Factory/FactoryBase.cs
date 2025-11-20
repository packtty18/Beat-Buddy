using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class FactoryBase : MonoBehaviour
{
    public abstract void InitFactory();
}
//팩토리의 베이스
public abstract class FactoryBase<TEnum> : FactoryBase where TEnum : Enum
{
    [SerializeField] protected Transform _rootTransform;

    [Header("Pooling")]
    [SerializeField] protected int _poolSize = 10;

    protected Dictionary<TEnum, GameObject> _prefabMap;   
    protected Dictionary<TEnum, GameObject[]> _poolMap;     

    protected virtual void Awake()
    {
        InitFactory();
    }

    public override void InitFactory()
    {
        _prefabMap = new Dictionary<TEnum, GameObject>();
        _poolMap = new Dictionary<TEnum, GameObject[]>();

        RegisterPrefabs();  // 자식 클래스에서 프리팹 등록
        InitializePools();  // 풀 생성
    }

    private void Start()
    {
        if (_rootTransform == null)
        {
            _rootTransform = transform;
        }
    }


    protected abstract void RegisterPrefabs();
    
    //풀 초기화
    protected virtual void InitializePools()
    {
        foreach (TEnum type in Enum.GetValues(typeof(TEnum)))
        {
            if(type.ToString() == "None")
            {
                continue;
            }

            _poolMap[type] = new GameObject[_poolSize];

            for (int i = 0; i < _poolSize; i++)
            {
                _poolMap[type][i] = CreateInstance(type);
            }
        }
    }

    //인스턴스 생성
    protected virtual GameObject CreateInstance(TEnum type)
    {
        if (!_prefabMap.ContainsKey(type))
        {
            Debug.LogError($"{GetType()} : Prefab not found for type: {type}");
            return null;
        }

        GameObject obj = Instantiate(_prefabMap[type]);
        obj.SetActive(false);
        obj.transform.SetParent(_rootTransform);
        return obj;
    }


    //풀 우선 반환 -> 리사이즈 및 추가생성
    protected virtual GameObject GetObjectFromPool(TEnum type)
    {
        GameObject[] pool = _poolMap[type];

        for (int i = 0; i < pool.Length; i++)
        {
            if (!pool[i].activeInHierarchy)
                return pool[i];
        }

        ResizePool(type);
        return GetObjectFromPool(type);
    }

    protected virtual void ResizePool(TEnum type)
    {
        GameObject[] oldPool = _poolMap[type];

        int additionalSize = Mathf.Max(oldPool.Length / 2 , 1);
        GameObject[] newPool = new GameObject[oldPool.Length + additionalSize];

        for (int i = 0; i < oldPool.Length; i++)
            newPool[i] = oldPool[i];

        for (int i = oldPool.Length; i < newPool.Length; i++)
            newPool[i] = CreateInstance(type);

        _poolMap[type] = newPool;

        Debug.Log($"[FactoryBase] Pool resized for {type} in {GetType().Name}");
    }

    public virtual GameObject CreateObject(TEnum type)
    {
        GameObject obj = GetObjectFromPool(type);
        return obj;
    }
}
