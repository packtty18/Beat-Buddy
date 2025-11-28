using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

public class BuddyManager : SceneSingleton<BuddyManager>
{
    [Header("버디 리스트")]
    [SerializeField] private List<GameObject> _buddyList = new List<GameObject>();
    [Header("현재 버디가 누구냐 (확인용)")]
    [SerializeField] private GameObject _currentBuddyPrefab;
    [Header("버디 스폰 위치")]
    [SerializeField] private Transform _buddySpawnPoint;
    private Vector3 _fireBuddySpawnVector = new Vector3(0, -0.5f, 0);
    private Vector3 _waterBuddySpawnVector = new Vector3(0, -0.75f, 0);
    [Header("노트 컨트롤용")]
    [SerializeField] private NoteController _noteController;

    private ESongType _currentBuddyType;
    private float _buddyDamage;
    private float _buddyMaxHealth;
    private BuddyStat _buddyStat;

    private Sequence _knockbackSequence;
    protected override void Awake()
    {
        base.Awake();
        _currentBuddyType = 0;
        if (StatManager.Instance != null)
        {
            StatManager.Instance.OnStatApplied += UpdateBuddyStat;
        }
    }

    public void SpawnBuddy()
    {
        // 현재 선택된 곡의 버디 타입 가져오기
        ESongType selectedSongType = SongManager.Instance.SelectedSongType;
        // 이미 스폰된 버디가 있는지 확인
        if (_currentBuddyType == selectedSongType)
        {
            Debug.Log("[BuddyManager] 이미 해당 버디가 스폰되어 있습니다.");
            return;
        }
        // 기존 버디 제거
        foreach (Transform child in _buddySpawnPoint)
        {
            Destroy(child.gameObject);
        }
        // 새로운 버디 스폰
        int buddyIndex = (int)selectedSongType - 1; // ESongType이 1부터 시작한다고 가정
        if (buddyIndex >= 0 && buddyIndex < _buddyList.Count)
        {
            BuddyInstantiate(_buddyList[buddyIndex], buddyIndex);
            _currentBuddyType = selectedSongType;
            _currentBuddyPrefab.transform.DOMoveX(_currentBuddyPrefab.transform.position.x - 5.6f, 3f);
            GetBuddyStat(_currentBuddyPrefab);
            GetBuddyEvent();
        }
    } 

    private void BuddyInstantiate(GameObject buddyPrefab, int buddyIndex)
    {
        switch(buddyIndex)
        {
            case 0:
                _currentBuddyPrefab = Instantiate(buddyPrefab, _buddySpawnPoint.position + _fireBuddySpawnVector, Quaternion.identity, _buddySpawnPoint);
                break;
            case 1:
                _currentBuddyPrefab = Instantiate(buddyPrefab, _buddySpawnPoint.position + _waterBuddySpawnVector, Quaternion.identity, _buddySpawnPoint);
                break;
            default:
                _currentBuddyPrefab = Instantiate(buddyPrefab, _buddySpawnPoint.position, Quaternion.identity, _buddySpawnPoint);
                break;
        }
    }

    private void GetBuddyEvent()
    {
        PlayerManager.Instance.OnAttackToBuddy += OnHitDamage;
    }

    private void OnHitDamage(float playerDamage)
    {
        _buddyStat.DecreaseHealth(playerDamage);
        _currentBuddyPrefab.GetComponent<BuddyAnimatorController>().OnHit();
        KnockBackBuddy();
    }

    private void GetBuddyStat(GameObject currentBuddyPrefab)
    {
        _buddyStat = _currentBuddyPrefab.GetComponent<BuddyStat>();
        StatManager.Instance.SetBuddyStat(_buddyStat);
    }
    private void UpdateBuddyStat()
    {
        if (_buddyStat != null)
        {
            _buddyDamage = _buddyStat.GetDamage();
            _buddyMaxHealth = _buddyStat.GetMaxHealth();
            Debug.Log($"[BuddyManager] Buddy Damage Updated: {_buddyDamage}");
        }
    }
    public float GetBuddyDamage()
    {
        return _buddyDamage;
    }
    public float GetMaxHealth()
    {
        return _buddyMaxHealth;
    }
    public void StartBuddyAttackAnimation(bool attacking)
    {
        _currentBuddyPrefab.GetComponent<BuddyAnimatorController>().OnAttack(attacking);
    }

    public NoteController GetNoteController()
    {
        return _noteController;
    }

    public bool IsBuddyDefeated()
    {
        if (_buddyStat != null)
        {
            return _buddyStat.isDefeated();
        }
        return false;
    }

    public void DefeatAnimation()
    {
        _currentBuddyPrefab.GetComponent<BuddyAnimatorController>().OnDefeated(true);
    }

    public void RunAwayAnimation()
    {
        _currentBuddyPrefab.transform.DORotate(new Vector3(0, 180, 0), 1f)
            .OnComplete(() => _currentBuddyPrefab.transform.DOMoveX(_currentBuddyPrefab.transform.position.x + 10f, 3f));
        _currentBuddyPrefab.GetComponent<BuddyAnimatorController>().OnAttack(true);
    }
    public void KnockBackBuddy()
    {
        if (_knockbackSequence != null && _knockbackSequence.IsActive())
            return;

        _knockbackSequence?.Kill();
        _knockbackSequence = Knockback.PlayKnockback(_currentBuddyPrefab.transform, -_currentBuddyPrefab.transform.right);
        _knockbackSequence.OnComplete(() => _knockbackSequence = null);
    }
}
