using UnityEngine;
using System.Collections;

public class FirePatternSpawner : MonoBehaviour
{
    [Header("스포너 위치")]
    [SerializeField] private GameObject _firePatternSpawner;
    private Transform _spawnerPosition;
    private float _spawnRangeX = 6.7f;
    private float _spawnRangeY = -3.8f;

    [Header("쿨타임")]
    private float _spawnCoolTime = 3f;

    [Header("좌우 랜덤 스폰")]
    private float _maxRate = 1f;
    private float _minRate = 0f;


    private void Start()
    {
        StartCoroutine(FirePatternSpawnCoroutine());
    }

    private IEnumerator FirePatternSpawnCoroutine()
    {
        yield return new WaitForSeconds(_spawnCoolTime);
        RandomSpawnPosition();
        SpawnPattern();
    }

    private void RandomSpawnPosition()
    {
        _spawnerPosition = transform;
        float _spawnPosition = Random.Range(_minRate, _maxRate);
        if (_spawnPosition < 0.5f)
        {
            _spawnerPosition.position = new Vector2(-_spawnRangeX, _spawnRangeY);
        }
        else
        {
            _spawnerPosition.position = new Vector2(_spawnRangeX, _spawnRangeY);
        }
    }

    private void SpawnPattern()
    {
        GameObject Pattern = Instantiate(_firePatternSpawner);
        Pattern.transform.position = _spawnerPosition.position;
    }
}
