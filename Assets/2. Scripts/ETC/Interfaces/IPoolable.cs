using UnityEngine;

public interface IPoolable
{
    void OnSpawn();  //활성화 시

    void OnDespawn();//비활성화 시
}
