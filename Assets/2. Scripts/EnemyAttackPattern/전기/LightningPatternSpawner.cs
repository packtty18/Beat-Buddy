using UnityEngine;
using System.Collections;

public class LightningPatternSpawner : MonoBehaviour
{
    [Header("쿨타임")]
    private float _spawnCoolTime = 9f;

    private void StartAttack()
    {
        StartCoroutine(LightningPatternSpawnCoroutine());
    }

    private IEnumerator LightningPatternSpawnCoroutine()
    {
        yield return new WaitForSeconds(_spawnCoolTime);
        SpawnLightningPattern();
    }

    private void SpawnLightningPattern()
    {
        LightningBossPattern startAttack = GetComponent<LightningBossPattern>();
        startAttack.Attack();
    }
}
