using System.Collections;
using UnityEngine;
using static FinalBossPatternSpawner;

public class EsperPatternSpawner : MonoBehaviour, IPatternSpawner
{
    [Header("쿨타임")]
    private float _startAttackTime = 12f;  // 기본 시작시간 12f
    private float _spawnCoolTime = 5f;     // 기본 쿨타임 17f (12 + 5)

    [Header("파이널보스 참조용")]
    public float PatternDuration => 11f;


    private void StartAttack()
    {
        StartCoroutine(EsperPatternSpawnCoroutine());
    }

    private IEnumerator EsperPatternSpawnCoroutine()
    {
        while (SongPlayManager.Instance.IsPlaying())
        {
            yield return new WaitForSeconds(_startAttackTime);
            SpawnEsperPattern();
            yield return new WaitForSeconds(_spawnCoolTime);
        }
    }

    public void SpawnEsperPattern()
    {
        EsperBossPattern startAttack = GetComponent<EsperBossPattern>();
        startAttack.Attack();
    }

    public void SpawnPattern()
    {
        SpawnEsperPattern();
    }
}
