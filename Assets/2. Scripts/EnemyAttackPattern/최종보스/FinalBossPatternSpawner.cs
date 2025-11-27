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
    [SerializeField] private bool _isFinalAttackActive = false;

    [Header("쿨타임")]
    private float _startAttackTime = 4.6f;  // 기본 시작시간 4.6f
    private float _spawnCoolTime = 1.8f;  // 기본 쿨타임 6.4f (4.6 + 1.8)
    private float _finishAttackCoolTime = 10f;  // 마지막 10초 전

    [Header("공격 횟수 관련")]
    private int _attackTimes = 0;
    private int _lastAttackTimes = 10;

    [Header("중복 스폰 방지")]
    private int _lastIndex = -1;


    public interface IPatternSpawner
    {
        void SpawnPattern();
        float PatternDuration { get; }
    }
    private void Awake()
    {
        StageManager.Instance.OnPlaySong -= StartAttack; // 중복 방지
        StageManager.Instance.OnPlaySong += StartAttack;
    }

    private void StartAttack()
    {
        StartCoroutine(FinalBossPatternSpawnCoroutine());
    }

    private IEnumerator FinalBossPatternSpawnCoroutine()
    {
        _isFinalAttackActive = true;

        while (SongPlayManager.Instance.IsPlaying())
        {
            yield return new WaitForSeconds(_startAttackTime);

            SpawnAttackPattern();

            if (_attackTimes == _lastAttackTimes)
            {
                yield return new WaitForSeconds(_finishAttackCoolTime); // 마지막 10초 전에 공격 중지
                break;
            }
            else 
            {
                yield return new WaitForSeconds(_spawnCoolTime);
            }
        }

        _isFinalAttackActive = false;
    }
    private void SpawnAttackPattern()
    {
        int randomIndex = Random.Range(0, _patternSpawnerPrefab.Length); // 추첨

        while (randomIndex == _lastIndex && _patternSpawnerPrefab.Length > 1) // 중복 시 재추첨
        {
            randomIndex = Random.Range(0, _patternSpawnerPrefab.Length);
        }
        _lastIndex = randomIndex;  // 직전에 스폰된 스포너 번호 저장

        GameObject spawnerPrefab = _patternSpawnerPrefab[randomIndex]; // 랜덤 스포너 생성

        // 패턴 후 스포너 삭제
        if (spawnerPrefab.GetComponent<IPatternSpawner>() != null)
        {
            GameObject spawnerInstance = Instantiate(spawnerPrefab);
            IPatternSpawner spawner = spawnerInstance.GetComponent<IPatternSpawner>();
            spawner.SpawnPattern();
            Destroy(spawnerInstance, spawner.PatternDuration);
        }

        _attackTimes += 1;
    }
}
