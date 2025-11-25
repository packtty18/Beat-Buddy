using UnityEngine;
using System.Collections;

public class EsperPatternSpawner : MonoBehaviour
{
    [Header("쿨타임")]
    private float _spawnCoolTime = 14f;

    private void Start()
    {
        StartCoroutine(EsperPatternSpawnCoroutine());
    }

    private IEnumerator EsperPatternSpawnCoroutine()
    {
        yield return new WaitForSeconds(_spawnCoolTime);
        SpawnEsperPattern();
    }

    private void SpawnEsperPattern()
    {
        EsperBossPattern startAttack = GetComponent<EsperBossPattern>();
        startAttack.Attack();
    }
}
