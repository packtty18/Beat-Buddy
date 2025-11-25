using UnityEngine;
using System.Collections;


public class WaterPatternSpawner : MonoBehaviour
{
    [Header("쿨타임")]
    private float _spawnCoolTime = 14f;  // 시작 시간과 쿨타임 시간이 14초로 같음


    private void Start()
    {
        StartCoroutine(WaterPatternSpawnCoroutine());
    }

    private IEnumerator WaterPatternSpawnCoroutine()
    {
        yield return new WaitForSeconds(_spawnCoolTime);
        SpawnWaterPattern();
    }

    private void SpawnWaterPattern()
    {
        WaterBossPattern startAttack = GetComponent<WaterBossPattern>();
        startAttack.Attack();
    }
}
