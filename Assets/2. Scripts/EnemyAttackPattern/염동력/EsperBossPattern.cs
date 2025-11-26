using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EsperBossPattern : MonoBehaviour
{
    [Header("염동력 공격 트리거")]
    [SerializeField] private bool _isMovingNotesGoing = false;

    [Header("리듬노트 프리팹")]
    private List<Note> _rhythmNotes;

    [Header("노트 위치")]
    private Vector2[] _noteOriginPositions;
    private Vector2 _noteCurrentPosition;

    [Header("노트 개수")]
    private int _noteNumbers;

    [Header("S자 이동 관련 옵션")]
    private float _noteAmplitude;  // 진폭
    private float _noteFrequency;  // 흔들리는 빈도
    private float _noteMovingTime = 6f;  // 흔들리는 기간
    private float _watingTrasnfer = 2f;

    [Header("진폭 랜덤값")]
    private float _minAmplitude = 0.8f;
    private float _maxAmplitude = 1.6f;

    [Header("빈도 랜덤값")]
    private float _minFrequency = 3f;
    private float _maxFrequency = 6f;

    private NoteController _noteController;
    private void Start()
    {
        //_noteController = BuddyManager.Instance.GetNoteController();
    }
    public void Attack()
    {
        StartCoroutine(StartEsperAttackCoroutine());
    }

    //시작 코루틴
    private IEnumerator StartEsperAttackCoroutine()
    {
        _isMovingNotesGoing = true;
        //BuddyManager.Instance.StartBuddyPattern(true);

        SetValue();
        EsperPatternEffect.MakeEsperEffect();
        yield return new WaitForSeconds(_watingTrasnfer);  // 화면 색변환 대기
        //SetNoteNumbers();
        //StartCoroutine(MovingNotesCoroutine());
        yield return new WaitForSeconds(_noteMovingTime);
        _isMovingNotesGoing = false;
        //BuddyManager.Instance.StartBuddyPattern(false);

    }

    // 초기화
    private void SetValue()
    {
        _noteAmplitude = Random.Range(_minAmplitude, _maxAmplitude); // 진폭 랜덤값 지정
        _noteFrequency = Random.Range(_minFrequency, _maxFrequency); // 빈도 랜덤값 지정
    }

    // 리듬노트 배열에 오브젝트 값을 입력해주는 메서드
    private void SetNoteNumbers()
    {
        _rhythmNotes = _noteController.GetRandomNotesByProgress(0.4f, 1f, 5);
        if (_rhythmNotes.Count == 0) return;

        _noteNumbers = _rhythmNotes.Count;
        _noteOriginPositions = new Vector2[_rhythmNotes.Count];
        for (int i = 0; i < _noteNumbers; i++)
        {
            _noteOriginPositions[i] = _rhythmNotes[i].transform.position;
        }
    }

    //  각 노트의 포지션 등을 지정해주는 코루틴
    private IEnumerator MovingNotesCoroutine()
    {
        while (_isMovingNotesGoing) 
        {
            for (int i = 0; i < _noteNumbers; i++)
            {
                NoteSMoving(i);
            }
            yield return null;
        }
    }

    // S자 이동 메서드
    private void NoteSMoving(int number)
    {
        _noteCurrentPosition = _rhythmNotes[number].transform.position;
        _noteCurrentPosition.y = _noteOriginPositions[number].y + Mathf.Sin(Time.time * _noteFrequency) * _noteAmplitude;

        _rhythmNotes[number].transform.position = _noteCurrentPosition;
    }
}
