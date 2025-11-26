using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : SceneSingleton<PlayerManager>
{
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private Transform _playerSpawnPoint;
    private GameObject _currentPlayerPrefab;
    private PlayerStat _playerStat;
    private BuddyManager _buddyManager;
    private ScoreManager _scoreManager;
    private float _scoreIncrease;

    public Action<float> OnAttackToBuddy;

    protected override void Awake()
    {
        base.Awake();
        
    }

    private void Start()
    {
        _playerStat = GetComponent<PlayerStat>();
        _scoreManager = ScoreManager.Instance;
    }

    public void SpawnPlayer()
    {
        if (_playerPrefab != null)
        {
            _currentPlayerPrefab = Instantiate(_playerPrefab, _playerSpawnPoint.position, Quaternion.identity, _playerSpawnPoint);
            _currentPlayerPrefab.transform.DOMoveX(_currentPlayerPrefab.transform.position.x + 5f, 3f);
            GetPlayerStat(_currentPlayerPrefab);
        }
        else
        {
            Debug.LogError("[PlayerManager] 플레이어 프리팹 또는 스폰 포인트가 없습니다!");
        }
    }

    private void GetPlayerStat(GameObject currentPlayerPrefab)
    {
        _playerStat = currentPlayerPrefab.GetComponent<PlayerStat>();
        _playerStat.StartAttack += OnAttack;
        _playerStat.IsFever += OnFever;
        _playerStat.OnHealthChanged += CheckGameOver;
        StatManager.Instance.SetPlayerStat(_playerStat);
        _buddyManager = BuddyManager.Instance;
    }

    private void OnFever(bool isFever)
    {
        _currentPlayerPrefab.GetComponent<PlayerAnimatorController>().SetFever(isFever);
    }
    private void OnAttack()
    {
        _currentPlayerPrefab.GetComponent<PlayerAnimatorController>().OnAttack();
        _playerStat.ResetAttackGuage();
        OnAttackToBuddy?.Invoke(_playerStat.GetDamage());
    }

    public void OnHit(EHitType hitType)
    {
        _scoreIncrease = hitType == EHitType.Perfect ? 10f : 5f;
        switch (hitType)
        {
            case EHitType.Perfect:
                GetPerfectGood(hitType);
                break;
            case EHitType.Good:
                GetPerfectGood(hitType);
                break;
            case EHitType.Bad:
                GetBadMiss();
                break;
            case EHitType.Miss:
                GetBadMiss();
                break;
            default:
                Debug.Log("Unknown Hit Type!");
                break;
        }
    }

    private void GetPerfectGood(EHitType hitType)
    {
        // 퍼펙트 굿 시
        _scoreManager.IncreaseScore(_scoreIncrease);
        _playerStat.OnHeal(hitType);
        _playerStat.IncreaseAttackGuage();
        _playerStat.IncreaseFeverGuage();
    }
    private void GetBadMiss()
    {
        // 배드 미스 시
        _playerStat.DecreaseHealth(_buddyManager.GetBuddyDamage());
        _currentPlayerPrefab.GetComponent<PlayerAnimatorController>().OnHit();
        _playerStat.ResetFeverGuage();
    }

    public void DefeatAnimation()
    {
        _currentPlayerPrefab.GetComponent<PlayerAnimatorController>().SetFail(true);
    }
    private void CheckGameOver(float currentHealth)
    {
        if (currentHealth <= 0)
        {
            StageManager.Instance.GameOver();
        }
    }
}
