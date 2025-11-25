using UnityEngine;
using System.Collections;

public class EsperPatternSpawner : MonoBehaviour
{
    [Header("쿨타임")]
    private float _startAttackTime = 4f;  // 패턴 시작 시간 12초
    private float _spawnCoolTime = 5f;     // 패턴 쿨타임 17초 (12 + 5)


    private void Start()
    {
        StartCoroutine(EsperPatternSpawnCoroutine());
    }

    private IEnumerator EsperPatternSpawnCoroutine()
    {
        yield return new WaitForSeconds(_startAttackTime);
        SpawnEsperPattern();
        yield return new WaitForSeconds(_spawnCoolTime);
    }

    public void SpawnEsperPattern()
    {
        EsperBossPattern startAttack = GetComponent<EsperBossPattern>();
        startAttack.Attack();
    }
}
