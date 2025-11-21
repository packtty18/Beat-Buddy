using UnityEngine;

[CreateAssetMenu(fileName = "NewBGMData", menuName = "Rhythm/BGM Data SO")]
public class BGMDataSO : ScriptableObject
{
    [Header("BGM 정보")]
    [Tooltip("곡 이름")]
    [SerializeField] private string _bgmName = "새로운 곡";

    [Tooltip("곡의 BPM")]
    [SerializeField] private float _bpm = 120f;

    [Tooltip("오디오 클립")]
    [SerializeField] private AudioClip _audioClip;

    [Header("노트 데이터")]
    [Tooltip("이 곡의 모든 노트 배열")]
    [SerializeField] private NoteData[] _notes = new NoteData[0];

    [Header("난이도 및 설정")]
    [Range(1, 5)]
    [SerializeField] private int _difficulty = 3;

    [Tooltip("곡 시작 전 대기 시간 (초)")]
    [SerializeField] private float _startDelay = 0f;

    [Header("노트 자동 생성 설정")]
    [Tooltip("시작 비트")]
    [SerializeField] private float _startBeat = 4f;

    [Tooltip("종료 비트")]
    [SerializeField] private float _endBeat = 100f;

    [Tooltip("노트 간격 (비트)")]
    [SerializeField] private float _noteInterval = 1f;

    public string BgmName => _bgmName;
    public float Bpm => _bpm;
    public AudioClip AudioClip => _audioClip;
    public NoteData[] Notes => _notes;
    public int Difficulty => _difficulty;
    public float StartDelay => _startDelay;

    // 곡의 총 길이 (초)
    public float GetDuration()
    {
        return _audioClip != null ? _audioClip.length : 0f;
    }

    // 유효성 검사
    public bool IsValid()
    {
        if (_audioClip == null)
        {
            Debug.LogError($"BGMDataSO [{_bgmName}]: AudioClip이 없습니다!");
            return false;
        }

        if (_notes == null || _notes.Length == 0)
        {
            Debug.LogWarning($"BGMDataSO [{_bgmName}]: 노트 데이터가 비어 있습니다!");
        }

        return true;
    }

    // 비트 간격으로 노트 자동 생성
    [ContextMenu("비트 간격으로 노트 자동 생성")]
    public void GenerateNotesByBeat()
    {
        var noteList = new System.Collections.Generic.List<NoteData>();

        for (float beat = _startBeat; beat <= _endBeat; beat += _noteInterval)
        {
            ENoteType type;

            // 랜덤 패턴
            type = (Random.value > 0.5f) ? ENoteType.LNote : ENoteType.RNote;
            noteList.Add(new NoteData(beat, type));
        }

        _notes = noteList.ToArray();

        Debug.Log($"노트 자동 생성 완료: {_notes.Length}개 ({_startBeat}~{_endBeat}비트, 간격: {_noteInterval})");
    }

    // 곡 길이 기반 자동 생성
    [ContextMenu("곡 길이 기반으로 노트 자동 생성")]
    public void GenerateNotesBySongLength()
    {
        if (_audioClip == null)
        {
            Debug.LogError("AudioClip이 없습니다!");
            return;
        }

        // 곡 길이를 비트로 변환
        float songLengthInSeconds = _audioClip.length;
        float totalBeats = (songLengthInSeconds / 60f) * _bpm;

        // 마지막 비트 자동 설정 (여유 2비트 제외)
        _endBeat = Mathf.Floor(totalBeats) - 2f;

        Debug.Log($"곡 길이: {songLengthInSeconds:F2}초 = {totalBeats:F2}비트");
        Debug.Log($"생성 범위: {_startBeat}~{_endBeat}비트");

        GenerateNotesByBeat();
    }

    // 패턴별 자동 생성
    [ContextMenu("패턴: 4비트마다 (느린 템포)")]
    public void GeneratePattern_Slow()
    {
        _noteInterval = 4f;
        GenerateNotesByBeat();
    }

    [ContextMenu("패턴: 2비트마다 (보통 템포)")]
    public void GeneratePattern_Normal()
    {
        _noteInterval = 2f;
        GenerateNotesByBeat();
    }

    [ContextMenu("패턴: 1비트마다 (빠른 템포)")]
    public void GeneratePattern_Fast()
    {
        _noteInterval = 1f;
        GenerateNotesByBeat();
    }

    [ContextMenu("패턴: 0.5비트마다 (매우 빠름)")]
    public void GeneratePattern_VeryFast()
    {
        _noteInterval = 0.5f;
        GenerateNotesByBeat();
    }

    [ContextMenu("노트 정렬")]
    public void SortNotes()
    {
        System.Array.Sort(_notes, (a, b) => a.beat.CompareTo(b.beat));
        Debug.Log($"{_bgmName}: {_notes.Length}개 노트 정렬 완료");
    }

    public NoteData GetNoteAtBeat(float beat, float tolerance = 0.1f)
    {
        foreach (var note in _notes)
        {
            if (Mathf.Abs(note.beat - beat) <= tolerance)
            {
                return note;
            }
        }
        return null;
    }

    public int GetTotalNotes() => _notes.Length;

    public int GetLNoteCount()
    {
        int count = 0;
        foreach (var note in _notes)
        {
            if (note.type == ENoteType.LNote) count++;
        }
        return count;
    }

    public int GetRNoteCount()
    {
        int count = 0;
        foreach (var note in _notes)
        {
            if (note.type == ENoteType.RNote) count++;
        }
        return count;
    }
}
