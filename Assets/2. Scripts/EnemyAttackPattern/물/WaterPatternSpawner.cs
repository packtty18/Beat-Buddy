using UnityEngine;
using System.Collections;


public class WaterPatternSpawner : MonoBehaviour
{
    [Header("쿨타임")]
    private float _spawnCoolTime = 5f;  // 기본 13f


    private void Start()
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
}
