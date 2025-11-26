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
    [SerializeField] private bool _isSpawning = true;

    [Header("쿨타임")]
    private float _startAttackTime = 4f;
    private float _spawnCoolTime = 9f;
    private float _fireAttackTime = 8f;
    private float _lightningAttackTime = 8f;
    private float _waterAttackTime = 12f;
    private float _esperAttackTime = 10f;

    [Header("보스 패턴 스폰 랜덤값")]
    private int _minBossRange = 0;
    private int _maxBossRange = 4;


    private void Start()
    {
        StartCoroutine(FinalBossPatternSpawnCoroutine());
    }

    private IEnumerator FinalBossPatternSpawnCoroutine()
    {
        while (_isSpawning)
        {
            yield return new WaitForSeconds(_startAttackTime);
            SpawnAttackPattern();
            yield return new WaitForSeconds(_spawnCoolTime);
        }

    }
    private void SpawnAttackPattern()
    {
        int randomBossNumber = Random.Range(_minBossRange, _maxBossRange);

        switch (randomBossNumber)
        {
            case 0:
                GameObject _fireSpawner = Instantiate(_patternSpawnerPrefab[randomBossNumber]);
                FirePatternSpawner fireAttack = _fireSpawner.GetComponent<FirePatternSpawner>();
                fireAttack.SpawnFirePattern();
                Destroy(_fireSpawner, _fireAttackTime);
                break;
            case 1:
                GameObject _lightningSpawner = Instantiate(_patternSpawnerPrefab[randomBossNumber]);
                LightningPatternSpawner lightningAttack = _lightningSpawner.GetComponent<LightningPatternSpawner>();
                lightningAttack.SpawnLightningPattern();
                Destroy(_lightningSpawner, _lightningAttackTime);
                break;
            case 2:
                GameObject _waterSpawner = Instantiate(_patternSpawnerPrefab[randomBossNumber]);
                WaterPatternSpawner waterAttack = _waterSpawner.GetComponent<WaterPatternSpawner>();
                waterAttack.SpawnWaterPattern();
                Destroy(_waterSpawner, _waterAttackTime);
                break;
            case 3:
                GameObject _esperSpawner = Instantiate(_patternSpawnerPrefab[randomBossNumber]);
                EsperPatternSpawner esperAttack = _esperSpawner.GetComponent<EsperPatternSpawner>();
                esperAttack.SpawnEsperPattern();
                Destroy(_esperSpawner, _esperAttackTime);
                break;
        }
    }
}
