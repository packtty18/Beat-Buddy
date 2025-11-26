using DG.Tweening;
using UnityEngine;

public class PlayerManager : SceneSingleton<PlayerManager>
{
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private Transform _playerSpawnPoint;
    private GameObject _currentPlayerPrefab;
    private PlayerStat _playerStat;
    private BuddyManager _buddyManager;
    private ScoreManager _scoreManager;
    private float _scoreIncrease;

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
        StatManager.Instance.SetPlayerStat(_playerStat);
        _buddyManager = BuddyManager.Instance;
    }

    private void OnAttack()
    {
        _playerStat.ResetAttackGuage();
    }

    public void OnHit(EHitType hitType)
    {
        _scoreIncrease = hitType == EHitType.Perfect ? 10f : 5f;
        switch (hitType)
        {
            case EHitType.Perfect:
                _scoreManager.IncreaseScore(_scoreIncrease);
                _playerStat.OnHeal(hitType);
                _playerStat.IncreaseAttackGuage();
                _playerStat.IncreaseFeverGuage();
                break;
            case EHitType.Good:
                _scoreManager.IncreaseScore(_scoreIncrease);
                _playerStat.OnHeal(hitType);
                _playerStat.IncreaseAttackGuage();
                _playerStat.IncreaseFeverGuage();
                break;
            case EHitType.Bad:
                break;
            case EHitType.Miss:
                _playerStat.DecreaseHealth(_buddyManager.GetComponent<BuddyStat>().GetDamage());
                _playerStat.ResetFeverGuage();
                break;
            default:
                Debug.Log("Unknown Hit Type!");
                break;
        }
    }
}
