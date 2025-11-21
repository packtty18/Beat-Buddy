using UnityEngine;
using UnityEngine.Rendering;

public class PoolableTestObject : MonoBehaviour, IPoolable
{
    public void OnDespawn()
    {
        Debug.Log("despawn");
        gameObject.SetActive(false);
    }

    public void OnSpawn()
    {
        Debug.Log("Spawn");
        
        gameObject.SetActive(true);
    }

}
