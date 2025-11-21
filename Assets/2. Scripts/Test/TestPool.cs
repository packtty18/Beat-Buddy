
using System.Collections.Generic;
using UnityEngine;

public class TestPool : PoolBase<ETestType>
{
    [SerializeField] private ETestType _testType;
    [SerializeField] private Vector3 _spawnPosition = Vector3.zero;
    [SerializeField] private Quaternion _spawnRotation = Quaternion.identity;

    Queue<TypeObjectPair<ETestType>> last = new Queue<TypeObjectPair<ETestType>>();


    public void SpawnTestObject()
    {
        GameObject obj = GetObject(_testType);
        obj.transform.position = _spawnPosition;
        obj.transform.rotation = _spawnRotation;
        obj.GetComponent<PoolableTestObject>().OnSpawn();
        last.Enqueue(new TypeObjectPair<ETestType>(_testType, obj));
    }

    public void ReturnTestObject()
    {
        if(last.Count == 0)
        {
            return;
        }
        TypeObjectPair<ETestType> target = last.Dequeue();
        target.Object.GetComponent<PoolableTestObject>().OnDespawn();
        ReturnObject(target.Type, target.Object );
    }
}
