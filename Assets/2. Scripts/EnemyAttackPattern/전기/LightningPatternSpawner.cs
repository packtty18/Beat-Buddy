using System.Collections;
using UnityEngine;
using static FinalBossPatternSpawner;

public class LightningPatternSpawner : MonoBehaviour, IPatternSpawner
{
    [Header("쿨타임")]
    private float _startAttackTime = 18f;  // 기본 시작시간 18f
    private float _spawnCoolTime = 14f;    // 기본 쿨타임 14f

    [Header("첫 공격 시간관련")]
    private bool _isStartAttack = false;

    [Header("파이널보스 참조용")]
    public float PatternDuration => 6f;


    private void StartAttack()
    {
        StartCoroutine(LightningPatternSpawnCoroutine());
    }

    private IEnumerator LightningPatternSpawnCoroutine()
    {
        while (SongPlayManager.Instance.IsPlaying())
        {
            if (_isStartAttack == false)
            {
                yield return new WaitForSeconds(_startAttackTime);
                _isStartAttack = true;
            }
            SpawnLightningPattern();
            yield return new WaitForSeconds(_spawnCoolTime);
        }
    }

    public void SpawnLightningPattern()
    {
        LightningBossPattern startAttack = GetComponent<LightningBossPattern>();
        startAttack.Attack();
    }

    public void SpawnPattern()
    {
        SpawnLightningPattern();
    }
}
