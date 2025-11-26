using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;


public class NoteEffect : MonoBehaviour, IPoolable
{
    private ENoteEffectType _effectType;
    public void SetEffectType(ENoteEffectType type)
    {
        _effectType = type;
    }
    public void OnSpawn()
    {
        GetComponent<ParticleSystem>().Play();
    }
    public void OnDespawn()
    {
        GetComponent<ParticleSystem>().Clear();
    }
    private void OnEnable()
    {
        StartCoroutine(Effecting());
    }
    private IEnumerator Effecting()
    {
        ParticleSystem ps = GetComponent<ParticleSystem>();
        yield return new WaitUntil(() => !ps.IsAlive(true));
        PoolManager.Instance.Despawn<NoteEffectPool, ENoteEffectType>(_effectType, gameObject);
    }
}
