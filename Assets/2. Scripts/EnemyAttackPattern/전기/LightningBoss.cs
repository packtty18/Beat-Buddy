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

    [Header("리듬노트 프리팹")]
    private GameObject[] _rhythmNotes;
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
    private bool _lightningSpawned1 = false;
    private bool _lightningSpawned2 = false;
    private bool _lightningSpawned3 = false;

    [Header("공격 패턴 반복")]
    private int _minAttackCount = 2;
    private int _maxAttackCount = 5;
    private bool _thunderRepeat = false;


    [Header("쿨타임")]
    private float _currentTime = 0f;
    private float _startLightning = 0.9f;
    private float _thunderAnimationTime = 0.34f;
    private float _lightningCurrentTime = 0f;
    private float _lightningAnimationTime = 0.3f;
    private float _double = 2f;


    private void Start()
    {
        // 초기화
        _lightningSpawned1 = false;
        _lightningSpawned2 = false;
        _lightningSpawned3 = false;
        _thunderSpawned = false;
        _thunderRepeat = false;
        _lightningCurrentTime = 0f;

        _leftNote = Resources.Load<GameObject>("LNote");
        _rightNote = Resources.Load<GameObject>("RNote");

        // 플래시 효과 발동
        FlashScreen.Flash();
    }

    private void Update()
    {
        StartLightningAttack();
    }

    // 공격 시작 메서드
    private void StartLightningAttack()
    {
        _currentTime += Time.deltaTime;
        if (_currentTime >= _startLightning) _lightningCurrentTime += Time.deltaTime; // 번개 친 이후 전기효과
        StartAttackEffect();
    }

    // 공격 효과 메서드
    private void StartAttackEffect()
    {
        if (_thunderRepeat == false) ThunderAttackRepeat();
        if (_currentTime >= _startLightning) LightningEffect();
    }

    // 번개 공격 반복 횟수 메서드
    private void ThunderAttackRepeat()
    {
        _thunderRepeat = true;
        int repeatCount = Random.Range(_minAttackCount, _maxAttackCount);
        for (int i = 0; i <= repeatCount; i++) 
        {
            _thunderSpawned = false;
            ThunderAttackRandom();
        }
    }

    // 번개 공격 노트 랜덤 타게팅 메서드
    private void ThunderAttackRandom()
    {
        GameObject[] _rhythmNotes = GameObject.FindGameObjectsWithTag("Respawn");  // 임시로 Respawn 태그 사용
        if (_rhythmNotes.Length == 0) return;

        GameObject target = _rhythmNotes[Random.Range(0, _rhythmNotes.Length)];
        ThunderAttack(target);
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
        //ChangeNote(closestRhythm);  // 노트 속성 변경 메서드. 아래 설명 참고
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
