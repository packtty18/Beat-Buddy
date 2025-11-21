using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TypeObjectPair<TEnum> where TEnum : Enum
{
    public TEnum Type;          // Enum 키
    public GameObject Object;   // 연결할 GameObject

    public TypeObjectPair(TEnum type, GameObject obj)
    {
        Type = type;
        Object = obj;
    }
}

public abstract class PoolBase : MonoBehaviour
{
    public abstract void InitPool();
}

//팩토리의 베이스
public abstract class PoolBase<TEnum> : PoolBase where TEnum : Enum
{
    [SerializeField] protected Transform _rootTransform;

    [Header("Pooling")]
    [SerializeField] protected int _poolSize = 10;
    [SerializeField] protected List<TypeObjectPair<TEnum>> _registPrefabList = new List<TypeObjectPair<TEnum>>();

    protected Dictionary<TEnum, GameObject> _prefabMap;
    protected Dictionary<TEnum, Queue<GameObject>> _poolMap;

    //해당 풀 초기화
    public override void InitPool()
    {
        _poolMap = new Dictionary<TEnum, Queue<GameObject>>();
        _rootTransform = _rootTransform == null ? transform : _rootTransform; 

        RegisterPrefabs();
        InitializePools();
    }

    //_registPrefabList에 등록된 타입과 프리팹으로 딕셔너리에 키값 쌍으로 연결
    private void RegisterPrefabs()
    {
        _prefabMap = new Dictionary<TEnum, GameObject>();
        foreach (var item in _registPrefabList)
        {
            if (item.Object == null)
            {
                continue;
            }

            if (!_prefabMap.ContainsKey(item.Type))
            {
                _prefabMap.Add(item.Type, item.Object);
            }
            else
            {
                Debug.LogWarning($"Duplicate enum key: {item.Type}");
            }
        }
    }

    //초기 풀 생성
    private  void InitializePools()
    {
        foreach (TEnum type in _prefabMap.Keys)
        {
            if (!IsPrefabRegistered(type))
            {
                Debug.LogError($"{GetType()} : Prefab not found for type: {type}");
                continue;
            }

            Queue<GameObject> queue = new Queue<GameObject>();

            for (int i = 0; i < _poolSize; i++)
            {
                GameObject target = CreateInstance(type);
                queue.Enqueue(target);
            }
            _poolMap[type] = queue;
        }
    }

    //유일한 Instantiate로 인스턴스화
    private GameObject CreateInstance(TEnum type)
    {
        GameObject obj = Instantiate(_prefabMap[type]);
        obj.SetActive(false);
        obj.transform.SetParent(_rootTransform);
        return obj;
    }

    private bool IsPrefabRegistered(TEnum type)
    {
        return _prefabMap.ContainsKey(type);
    }

    private bool IsTypeContainedInPoolMap(TEnum type)
    {
        return _poolMap.ContainsKey(type);
    }


    private void ResizePool(TEnum type)
    {
        int additionalSize = Mathf.Max(_poolSize, 1);
        Queue<GameObject> queue = _poolMap[type];

        for (int i = 0; i < additionalSize; i++)
        {
            queue.Enqueue(CreateInstance(type));
        }

        Debug.Log($"[Poolbase] Pool resized for {type} in {GetType().Name}");
    }


    /// <summary>
    /// 풀(큐)에서 오브젝트 가져오기
    /// </summary>
    public GameObject GetObject(TEnum type)
    {
        if (!IsTypeContainedInPoolMap(type))
        {
            Debug.LogError($"Pool not found for type: {type}");
            return null;
        }

        Queue<GameObject> queue = _poolMap[type];

        // 큐가 비어있으면 풀 확장
        if (queue.Count == 0)
        {
            ResizePool(type);
        }

        GameObject obj = queue.Dequeue();
        obj.SetActive(true);
        return obj;
    }

    /// <summary>
    /// 사용 후 객체를 다시 큐에 넣기
    /// </summary>
    public void ReturnObject(TEnum type, GameObject target)
    {
        if (!IsTypeContainedInPoolMap(type))
        {
            Debug.LogError($"Pool not found for type: {type}");
            return;
        }

        Queue<GameObject> queue = _poolMap[type];
        target.SetActive(false);
        queue.Enqueue(target);
    }


    /// <summary>
    /// 현재 풀 상태를 로그로 출력 (각 타입별 큐 크기)
    /// </summary>
    [ContextMenu("Debug Pool")]
    protected void DebugPoolQueue()
    {
        if (_poolMap == null)
        {
            Debug.Log("[PoolBase] Pool map is null.");
            return;
        }

        foreach (var kvp in _poolMap)
        {
            TEnum type = kvp.Key;
            Queue<GameObject> queue = kvp.Value;
            int activeCount = queue.Count;

            Debug.Log($"[PoolBase] Type: {type}, Objects in Pool: {activeCount}");
        }
    }

}