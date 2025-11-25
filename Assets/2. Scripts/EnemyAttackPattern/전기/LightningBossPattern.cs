using System.Collections;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public enum ELightningPatternType
{
    Lightning,
    ThunderArrow,
    Thunder
}

public class LightningBossPattern : MonoBehaviour
{
    [Header("전기 공격 트리거")]
    public bool _isLightningAttackActive = false;

    [Header("패턴 프리팹")]
    [SerializeField] private GameObject[] _lightningPrefab;
    private GameObject _lightning;
    private GameObject _thunder;
    private GameObject _thunderArrow;

    [Header("리듬노트 프리팹")]
    private GameObject[] _rhythmNotes;
    private GameObject _rhythmNote;
    private GameObject _leftNote;
    private GameObject _rightNote;
    //public ENoteType NoteType; Enum에서 노트 타입 불러올때 사용. 일단 주석처리

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

    [Header("애니메이션 시간 관련")]
    private float _startLightning = 0.9f;
    private float _thunderAnimationTime = 0.34f;
    private float _lightningAnimationTime = 0.3f;


    public void Attack()
    {
        StartCoroutine(StartLightningAttackCoroutine());
    }

    private IEnumerator StartLightningAttackCoroutine()
    {
        _isLightningAttackActive = true;

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
            ThunderAttack(_rhythmNote);
            //ChangeNote(_rhythmNote);  // 노트 속성 변경 메서드. 아래 설명 참고
        }
    }

    // 번개 공격 노트 랜덤 타게팅 메서드
    private void RandomTargetingNotes()
    {
        GameObject[] _rhythmNotes = GameObject.FindGameObjectsWithTag("Respawn");  // 임시로 Respawn 태그 사용
        if (_rhythmNotes.Length == 0) return;

        GameObject target = _rhythmNotes[Random.Range(0, _rhythmNotes.Length)];
        _rhythmNote= target;
    }

    // 번개 효과 메서드
    private void ThunderAttack(GameObject note)
    {
        GameObject closestRhythm = note;
        _thunderPosition = closestRhythm.transform;
        _thunder = Instantiate(_lightningPrefab[(int)ELightningPatternType.Thunder], _thunderPosition);
        _thunderArrow = Instantiate(_lightningPrefab[(int)ELightningPatternType.ThunderArrow], _thunderPosition);
        Destroy(_thunder, _thunderAnimationTime);
        Destroy(_thunderArrow, _thunderAnimationTime);
        _thunderSpawned = true;
    }

    // 노트 속성을 반대로 변경하는 메서드
    // 추후 NoteController의 함수를 가져다 쓸 예정. 그 이전까진 주석처리.
    //private void ChangeNote(GameObject noteObject)
    //{
    //    Note note = noteObject.GetComponent<Note>();
    //    if (note == null)
    //    {
    //        Debug.LogWarning("Note 컴포넌트를 찾을 수 없습니다.");
    //        return;
    //    }

    //    if (note.NoteType == ENoteType.LNote)
    //    {
    //        ENoteType newType = ENoteType.RNote;
    //        note.ChangeType(newType);
    //    }
    //    else
    //    {
    //        ENoteType newType = ENoteType.LNote;
    //        note.ChangeType(newType);
    //    }
    //}

    // 흐르는 전기 소환
    private void SpawnLightning(Transform position)
    {
        _lightning = Instantiate(_lightningPrefab[(int)ELightningPatternType.Lightning], position);
        Destroy(_lightning, _lightningAnimationTime);
    }
}
