using UnityEngine;
using System.Collections;

public class LightningPatternSpawner : MonoBehaviour
{
    [Header("쿨타임")]
    private float _startAttackTime = 18f;  // 패턴 시작 시간 18초
    private float _spawnCoolTime = 10f;    // 패턴 쿨타임 28초  (18 + 10)


    private void Start()
    {
        StartCoroutine(LightningPatternSpawnCoroutine());
    }

    private IEnumerator LightningPatternSpawnCoroutine()
    {
        yield return new WaitForSeconds(_startAttackTime);
        SpawnLightningPattern();
        yield return new WaitForSeconds(_spawnCoolTime);
    }

    public void SpawnLightningPattern()
    {
        LightningBossPattern startAttack = GetComponent<LightningBossPattern>();
        startAttack.Attack();
    }
}
