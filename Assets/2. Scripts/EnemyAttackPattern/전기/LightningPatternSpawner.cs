using UnityEngine;
using System.Collections;

public class LightningPatternSpawner : MonoBehaviour
{
    [Header("쿨타임")]
    private float _startAttackTime = 2f;  // 기본 17f
    private float _spawnCoolTime = 8f;    // 기본 쿨타임 27f  (17 + 10)


    private void Start()
    {
        StartCoroutine(LightningPatternSpawnCoroutine());
    }

    private IEnumerator LightningPatternSpawnCoroutine()
    {
        //while (SongPlayManager.Instance.IsPlaying())
        //
            yield return new WaitForSeconds(_startAttackTime);
            SpawnLightningPattern();
            yield return new WaitForSeconds(_spawnCoolTime);
        //}
    }

    public void SpawnLightningPattern()
    {
        LightningBossPattern startAttack = GetComponent<LightningBossPattern>();
        startAttack.Attack();
    }
}
