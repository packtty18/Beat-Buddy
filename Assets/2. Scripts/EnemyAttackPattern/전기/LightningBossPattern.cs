using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ELightningPatternType
{
    Lightning,
    ThunderArrow,
    Thunder
}

public class LightningBossPattern : MonoBehaviour
{
    [Header("전기 공격 트리거")]
    [SerializeField] private bool _isLightningAttackActive = false;

    [Header("패턴 프리팹")]
    [SerializeField] private GameObject[] _lightningPrefab;
    private GameObject _lightning;
    private GameObject _thunder;
    private GameObject _thunderArrow;

    [Header("리듬노트 프리팹")]
    private List<Note> _rhythmNotes;
    private Note _rhythmNote;
    private GameObject _leftNote;
    private GameObject _rightNote;

    [Header("이펙트 포지션")]
    private Transform _thunderPosition;
    [SerializeField] private Transform _lightningPosition1;
    [SerializeField] private Transform _lightningPosition2;
    [SerializeField] private Transform _lightningPosition3;

    [Header("라이트닝 스폰")]
    private bool _thunderSpawned = false;

    [Header("공격 패턴 반복")]
    private int _minAttackCount = 2;
    private int _maxAttackCount = 5;
    private bool _thunderRepeat = false;

    [Header("번개 범위")]
    private float _minScreenX = -8f;
    private float _maxScreenX = 8f;
    private float _minScreenY = -3f;
    private float _maxScreenY = 4f;
    private Vector2 _lightningPosition;

    [Header("애니메이션 시간 관련")]
    private float _startLightning = 0.9f;
    private float _thunderAnimationTime = 0.34f;
    private float _lightningAnimationTime = 0.3f;

    private NoteController _noteController;
    private void Start()
    {
        _noteController = BuddyManager.Instance.GetNoteController();
    }
    public void Attack()
    {
        StartCoroutine(StartLightningAttackCoroutine());
    }

    private IEnumerator StartLightningAttackCoroutine()
    {
        _isLightningAttackActive = true;
        BuddyManager.Instance.StartBuddyPattern(true);

        // 초기화
        SetValue();

        // 플래시 효과 발동
        FlashScreen.Flash();
        StartLightningAttack();

        yield return new WaitForSeconds(_startLightning);
        SpawnLightning(_lightningPosition1);
        yield return new WaitForSeconds(_lightningAnimationTime);
        SpawnLightning(_lightningPosition2);
        yield return new WaitForSeconds(_lightningAnimationTime);
        SpawnLightning(_lightningPosition3);

        _isLightningAttackActive = false;
        BuddyManager.Instance.StartBuddyPattern(false);

    }

    private void SetValue()
    {
        // 초기화
        _thunderSpawned = false;
        _thunderRepeat = false;

        _leftNote = Resources.Load<GameObject>("LNote");
        _rightNote = Resources.Load<GameObject>("RNote");
    }

    // 공격 시작 메서드
    private void StartLightningAttack()
    {
        if (_thunderRepeat == false) ThunderAttackRepeat();
    }

    // 번개 공격 반복 횟수 메서드
    private void ThunderAttackRepeat()
    {
        _thunderRepeat = true;
        int repeatCount = Random.Range(_minAttackCount, _maxAttackCount);
        for (int i = 0; i <= repeatCount; i++) 
        {
            RandomTargetingNotes();
            if (_thunderSpawned == false) ThunderAttack();
        }
        _thunderSpawned = true;
    }

    // 번개 공격 노트 랜덤 타게팅 메서드
    private void RandomTargetingNotes()
    {
        _rhythmNotes = _noteController.GetRandomNotesByProgress(0.4f, 1f, 5);

        if (_rhythmNotes.Count == 0) return;

        Note target = _rhythmNotes[Random.Range(0, _rhythmNotes.Count)];
        _rhythmNote= target;
    }

    // 번개 효과 메서드
    private void ThunderAttack()
    {
        Vector2 newPosition = new Vector2(Random.Range(_minScreenX, _maxScreenX), Random.Range(_minScreenY, _maxScreenY));
        _lightningPosition = newPosition;
        _thunder = Instantiate(_lightningPrefab[(int)ELightningPatternType.Thunder]);
        _thunderArrow = Instantiate(_lightningPrefab[(int)ELightningPatternType.ThunderArrow]);
        _thunder.transform.position = newPosition;
        _thunderArrow.transform.position = newPosition;
        Destroy(_thunder, _thunderAnimationTime);
        Destroy(_thunderArrow, _thunderAnimationTime);
    }

    // 흐르는 전기 이펙트 소환 메서드
    private void SpawnLightning(Transform position)
    {
        _lightning = Instantiate(_lightningPrefab[(int)ELightningPatternType.Lightning], position);
        Destroy(_lightning, _lightningAnimationTime);
    }
}
