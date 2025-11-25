using UnityEngine;
using System.Collections;

public class FirePatternSpawner : MonoBehaviour
{
    [Header("스포너 위치")]
    [SerializeField] private GameObject _firePatternSpawner;
    private Transform _spawnerPosition;
    private float _spawnRangeX = 6.7f;

    [Header("쿨타임")]
    private float _startAttackTime = 12f;  // 패턴 시작 시간 12초
    private float _spawnCoolTime = 6f;     // 패턴 쿨타임 18초


    [Header("좌우 랜덤 스폰")]
    private float _maxRate = 1f;
    private float _minRate = 0f;


    private void StartAttack()
    {
        StartCoroutine(FirePatternSpawnCoroutine());
    }

    private IEnumerator FirePatternSpawnCoroutine()
    {
        yield return new WaitForSeconds(_startAttackTime);
        SpawnFirePattern();
        yield return new WaitForSeconds(_spawnCoolTime);
    }

    private void SpawnFirePattern()
    {
        _spawnerPosition = transform;
        float _spawnPosition = Random.Range(_minRate, _maxRate);
        if (_spawnPosition < 0.5f)
        {
            _spawnerPosition.position = new Vector2(-_spawnRangeX, _spawnerPosition.position.y);
        }
        else
        {
            _spawnerPosition.position = new Vector2(_spawnRangeX, _spawnerPosition.position.y);
        }
        FireBossPattern startAttack = GetComponent<FireBossPattern>();
        startAttack.Attack();
    }
}
