
using System.Collections.Generic;
using UnityEngine;

public class TestPool : PoolBase<ETestType>
{
    [SerializeField] private ETestType _testType;
    [SerializeField] private Vector3 _spawnPosition = Vector3.zero;
    [SerializeField] private Quaternion _spawnRotation = Quaternion.identity;

    Queue<TypeObjectPair<ETestType>> _spawnQueue = new Queue<TypeObjectPair<ETestType>>();


    public void SpawnTestObject()
    {
        GameObject obj = GetObject(_testType);
        obj.transform.position = _spawnPosition;
        obj.transform.rotation = _spawnRotation;
        _spawnQueue.Enqueue(new TypeObjectPair<ETestType>(_testType, obj));
    }

    public void ReturnTestObject()
    {
        if(_spawnQueue.Count == 0)
        {
            return;
        }
        TypeObjectPair<ETestType> target = _spawnQueue.Dequeue();
        
        ReturnObject(target.Type, target.Object );
    }
}
