using DG.Tweening;
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
    [Header("노트 컨트롤용")]
    [SerializeField] private NoteController _noteController;

    private ESongType _currentBuddyType;
    private float _buddyDamage;
    private BuddyStat _buddyStat;

    protected override void Awake()
    {
        base.Awake();
        _currentBuddyType = 0;
    }

    public float GetBuddyDamage()
    {
        return _buddyDamage;
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
            GameObject buddyPrefab = _buddyList[buddyIndex];
            _currentBuddyPrefab = Instantiate(buddyPrefab, _buddySpawnPoint.position, Quaternion.identity, _buddySpawnPoint);
            _currentBuddyType = selectedSongType;
            _currentBuddyPrefab.transform.DOMoveX(_currentBuddyPrefab.transform.position.x - 5f, 5f);
            GetBuddyStat(_currentBuddyPrefab);
        }
    } 

    private void GetBuddyStat(GameObject currentBuddyPrefab)
    {
        _buddyStat = _currentBuddyPrefab.GetComponent<BuddyStat>();
        _buddyDamage = _buddyStat.GetDamage();
        StatManager.Instance.SetBuddyStat(_buddyStat);
    }

    public void StartBuddyPattern(bool attacking)
    {
        _currentBuddyPrefab.GetComponent<BuddyAnimatorController>().OnAttack(attacking);
    }

    public NoteController GetNoteController()
    {
        return _noteController;
    }
}
