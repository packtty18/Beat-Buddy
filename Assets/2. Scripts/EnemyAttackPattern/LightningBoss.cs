using UnityEngine;
using UnityEngine.UI;

public enum ELightningPatternType
{
    Lightning,
    ThunderArrow,
    Thunder
}


public class LightningBoss : MonoBehaviour
{
    [Header("패턴 프리팹")]
    [SerializeField] private GameObject[] _lightningPrefab;
    private GameObject _lightning;
    private GameObject _thunder;
    private GameObject _thunderArrow;

    [Header("이펙트 포지션")]
    private Transform _thunderPosition;
    [SerializeField] private Transform _lightningPosition1;
    [SerializeField] private Transform _lightningPosition2;
    [SerializeField] private Transform _lightningPosition3;

    [Header("라이트닝 스폰")]
    private bool _thunderSpawned = false;
    private bool _lightningSpawned1 = false;
    private bool _lightningSpawned2 = false;
    private bool _lightningSpawned3 = false;

    [Header("쿨타임")]
    private float _thunderAnimationTime = 0.34f;
    private float _lightningAnimationTime = 0.3f;
    private float _lightningCurrentTime = 0f;
    private float _double = 2f;
    private float _currentTime = 0f;
    private float _startLightning = 0.9f;


    private void Start()
    {
        // 초기화
        _lightningSpawned1 = false;
        _lightningSpawned2 = false;
        _lightningSpawned3 = false;
        _thunderSpawned = false;
        _lightningCurrentTime = 0f;

        // 플래시 효과 발동
        FlashScreen.Flash();
    }

    private void Update()
    {
        _currentTime += Time.deltaTime;
        if (_currentTime >= _startLightning) _lightningCurrentTime += Time.deltaTime; // 번개 친 이후 전기효과
        StartAttackEffect();
    }


    // 공격 효과 메서드
    private void StartAttackEffect()
    {
        if (_thunderSpawned == false) ThunderAttack();
        if(_currentTime >= _startLightning) LightningEffect();
    }

    // 번개 효과 메서드
    private void ThunderAttack()
    {
        GameObject closestRhythm = GameObject.FindWithTag("Respawn");  // 테스트용 태그로 Respawn 사용
        _thunderPosition = closestRhythm.transform;
        _thunder = Instantiate(_lightningPrefab[(int)ELightningPatternType.Thunder], _thunderPosition);
        _thunderArrow = Instantiate(_lightningPrefab[(int)ELightningPatternType.ThunderArrow], _thunderPosition);
        Destroy(_thunder, _thunderAnimationTime);
        Destroy(_thunderArrow, _thunderAnimationTime);
        _thunderSpawned = true;
    }

    // 흐르는 전기 효과 메서드
    private void LightningEffect()
    {
        if (!_lightningSpawned1)
        {
            SpawnLightning(_lightningPosition1);
            _lightningSpawned1 = true;
        }
        if (!_lightningSpawned2 && _lightningCurrentTime >= _lightningAnimationTime)
        {
            SpawnLightning(_lightningPosition2);
            _lightningSpawned2 = true;
        }
        if (!_lightningSpawned3 && _lightningCurrentTime >= _lightningAnimationTime * _double)
        {
            SpawnLightning(_lightningPosition3);
            _lightningSpawned3 = true;
            enabled = false;
        }
    }

    // 흐르는 전기 소환
    private void SpawnLightning(Transform position)
    {
        _lightning = Instantiate(_lightningPrefab[(int)ELightningPatternType.Lightning], position);
        Destroy(_lightning, _lightningAnimationTime);
    }
}