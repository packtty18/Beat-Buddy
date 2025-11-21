using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 좌우에서 노트를 생성하는 시스템
/// </summary>
public class NoteSpawner : MonoBehaviour
{
    [Header("노트 프리팹")]
    [SerializeField] private GameObject _notePrefab;

    [Header("스폰 위치")]
    [SerializeField] private Transform _leftSpawnPoint;
    [SerializeField] private Transform _rightSpawnPoint;
    [SerializeField] private Transform _judgePoint;

    [Header("타이밍 설정")]
    [SerializeField] private float _beatsShownInAdvance = 3f;

    // BGMDataSO는 Inspector에서 설정 안 함! Conductor에서 가져옴
    private BGMDataSO _currentBGMData;

    private int _nextNoteIndex = 0;
    private List<Note> _activeNotes = new List<Note>();

    private Queue<GameObject> _notePool = new Queue<GameObject>();
    private int _poolSize = 50;

    void Start()
    {
        InitializePool();
        LoadBGMDataFromConductor();  // Conductor에서 가져오기
    }

    void Update()
    {
        if (_currentBGMData == null || _nextNoteIndex >= _currentBGMData.Notes.Length)
            return;

        float currentBeat = Conductor.Instance.BgmPositionInBeats;

        while (_nextNoteIndex < _currentBGMData.Notes.Length)
        {
            NoteData nextNote = _currentBGMData.Notes[_nextNoteIndex];
            float spawnBeat = nextNote.beat - _beatsShownInAdvance;

            if (currentBeat >= spawnBeat)
            {
                SpawnNote(nextNote);
                _nextNoteIndex++;
            }
            else
            {
                break;
            }
        }
    }

    void InitializePool()
    {
        for (int i = 0; i < _poolSize; i++)
        {
            GameObject note = Instantiate(_notePrefab, transform);
            note.SetActive(false);
            _notePool.Enqueue(note);
        }

        Debug.Log($"노트 풀 초기화: {_poolSize}개");
    }

    GameObject GetNoteFromPool()
    {
        if (_notePool.Count > 0)
        {
            GameObject note = _notePool.Dequeue();
            note.SetActive(true);
            return note;
        }
        else
        {
            Debug.LogWarning("노트 풀 부족! 새 노트 생성");
            return Instantiate(_notePrefab, transform);
        }
    }

    public void ReturnNoteToPool(GameObject note)
    {
        note.SetActive(false);
        _notePool.Enqueue(note);
    }

    /// <summary>
    /// Conductor로부터 BGMData 가져오기 (새로 추가)
    /// </summary>
    void LoadBGMDataFromConductor()
    {
        if (Conductor.Instance == null)
        {
            Debug.LogError("Conductor가 없습니다!");
            return;
        }

        _currentBGMData = Conductor.Instance.CurrentBGMData;

        if (_currentBGMData == null)
        {
            Debug.LogError("Conductor에 BGMDataSO가 설정되지 않았습니다!");
            return;
        }

        _nextNoteIndex = 0;
        ClearAllNotes();
        _currentBGMData.SortNotes();

        Debug.Log($"BGM 데이터 로드 (Conductor): {_currentBGMData.BgmName}, {_currentBGMData.Notes.Length}개 노트");
    }

    /// <summary>
    /// 외부에서 BGMData 변경 시 호출 (곡 전환용)
    /// </summary>
    public void ReloadBGMData()
    {
        LoadBGMDataFromConductor();
    }

    void SpawnNote(NoteData noteData)
    {
        GameObject noteObj = GetNoteFromPool();

        Transform spawnPos = (noteData.type == ENoteType.LNote)
            ? _leftSpawnPoint
            : _rightSpawnPoint;

        noteObj.transform.position = spawnPos.position;

        Note noteScript = noteObj.GetComponent<Note>();
        if (noteScript != null)
        {
            noteScript.Initialize(noteData.beat, noteData.type, _beatsShownInAdvance, _judgePoint);
            _activeNotes.Add(noteScript);
        }
        else
        {
            Debug.LogError("Note 프리팹에 Note 컴포넌트가 없습니다!");
        }
    }

    public void ClearAllNotes()
    {
        foreach (Note note in _activeNotes)
        {
            if (note != null && note.gameObject != null)
            {
                ReturnNoteToPool(note.gameObject);
            }
        }
        _activeNotes.Clear();
    }

    public List<Note> GetActiveNotes()
    {
        _activeNotes.RemoveAll(note => note == null || !note.gameObject.activeSelf);
        return _activeNotes;
    }

    public void RemoveNote(Note note)
    {
        _activeNotes.Remove(note);
        ReturnNoteToPool(note.gameObject);
    }
}
