using UnityEngine;

public class PoolableTestObject : MonoBehaviour, IPoolable
{
    public void OnDespawn()
    {
        Debug.Log("despawn");
        
    }

    public void OnSpawn()
    {
        Debug.Log("Spawn");
        
        gameObject.SetActive(true);
    }

}
