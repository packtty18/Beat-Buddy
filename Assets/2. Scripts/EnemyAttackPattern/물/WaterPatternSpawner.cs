using System.Collections;
using UnityEngine;
using static FinalBossPatternSpawner;


public class WaterPatternSpawner : MonoBehaviour, IPatternSpawner
{
    [Header("쿨타임")]
    private float _spawnCoolTime = 13f;  // 기본 쿨타임 13f

    [Header("파이널보스 참조용")]
    public float PatternDuration => 11f;

    private void Awake()
    {
        StageManager.Instance.OnPlaySong += StartAttack;
    }
    private void StartAttack()
    {
        StartCoroutine(WaterPatternSpawnCoroutine());
    }

    private IEnumerator WaterPatternSpawnCoroutine()
    {
        while(SongPlayManager.Instance.IsPlaying())
        {
            yield return new WaitForSeconds(_spawnCoolTime);
            SpawnWaterPattern();
        }
    }

    public void SpawnWaterPattern()
    {
        WaterBossPattern startAttack = GetComponent<WaterBossPattern>();
        startAttack.Attack();
    }

    public void SpawnPattern()
    {
        SpawnWaterPattern();
    }
}
