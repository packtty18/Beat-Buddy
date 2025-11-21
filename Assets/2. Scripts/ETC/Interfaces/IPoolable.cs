using UnityEngine;

public interface IPoolable
{
    //활성화 시 호출되는 메서드.
    void OnSpawn();

    //비활성화 시 호출하는 메서드.
    void OnDespawn();
}
