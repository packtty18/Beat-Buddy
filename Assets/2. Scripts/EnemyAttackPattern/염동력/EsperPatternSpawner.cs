using UnityEngine;
using System.Collections;

public class EsperPatternSpawner : MonoBehaviour
{
    [Header("쿨타임")]
    private float _startAttackTime = 12f;  // 기본 시작시간 12f
    private float _spawnCoolTime = 5f;     // 기본 쿨타임 5f (12 + 5)


    private void Start()
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
}
