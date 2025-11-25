using UnityEngine;
using System.Collections;

public enum EPatternType
{
    Fire,
    Lightning,
    Water,
    Esper
}

public class FinalBossPatternSpawner : MonoBehaviour
{
    [Header("스포너 오브젝트")]
    [SerializeField] private GameObject[] _patternSpawnerPrefab;

    [Header("쿨타임")]
    private float _spawnCoolTime = 3f;

    [Header("보스 패턴 스폰 랜덤값")]
    private int _minBossRange = 0;
    private int _maxBossRange = 4;


    private void Start()
    {
        StartCoroutine(FinalBossPatternSpawnCoroutine());
    }

    private IEnumerator FinalBossPatternSpawnCoroutine()
    {
        yield return new WaitForSeconds(_spawnCoolTime);
        SpawnAttackPattern();
    }
    private void SpawnAttackPattern()
    {
        int randomBossNumber = (int)Random.Range(_minBossRange, _maxBossRange);

        switch (randomBossNumber)
        {
            case 0:
                GameObject _fireSpawner = Instantiate(_patternSpawnerPrefab[randomBossNumber]);
                FirePatternSpawner fireAttack = _fireSpawner.GetComponent<FirePatternSpawner>();
                fireAttack.SpawnFirePattern();
                break;
            case 1:
                GameObject _lightningSpawner = Instantiate(_patternSpawnerPrefab[randomBossNumber]);
                LightningPatternSpawner lightningAttack = _lightningSpawner.GetComponent<LightningPatternSpawner>();
                lightningAttack.SpawnLightningPattern();
                break;
            case 2:
                GameObject _waterSpawner = Instantiate(_patternSpawnerPrefab[randomBossNumber]);
                WaterPatternSpawner waterAttack = _waterSpawner.GetComponent<WaterPatternSpawner>();
                waterAttack.SpawnWaterPattern();
                break;
            case 3:
                GameObject _esperSpawner = Instantiate(_patternSpawnerPrefab[randomBossNumber]);
                EsperPatternSpawner esperAttack = _esperSpawner.GetComponent<EsperPatternSpawner>();
                esperAttack.SpawnEsperPattern();
                break;
        }
    }
}
