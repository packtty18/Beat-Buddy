using System.Collections.Generic;
using UnityEngine;

public class NoteSpawner : MonoBehaviour
{
    [Header("노트 설정")]
    [SerializeField] private GameObject _notePrefab;
    [SerializeField] private Transform _leftSpawnPoint;
    [SerializeField] private Transform _rightSpawnPoint;
    [SerializeField] private Transform _judgePoint;
    [SerializeField] private float _beatsShownInAdvance = 3f;

    private BGMDataSO _currentBGMData;
    private int _nextNoteIndex = 0;
    private List<Note> _activeNotes = new List<Note>();
    private Queue<GameObject> _notePool = new Queue<GameObject>();

    void Start()
    {
        InitializePool(50);
        LoadBGMDataFromConductor();
    }

    void Update()
    {
        if (_currentBGMData == null || _nextNoteIndex >= _currentBGMData.Notes.Length) return;

        float currentBeat = Conductor.Instance.BgmPositionInBeats;

        while (_nextNoteIndex < _currentBGMData.Notes.Length)
        {
            NoteData nextNote = _currentBGMData.Notes[_nextNoteIndex];
            float spawnBeat = Mathf.Max(0f, nextNote.beat - _beatsShownInAdvance);

            if (currentBeat >= spawnBeat)
            {
                SpawnNote(nextNote);
                _nextNoteIndex++;
            }
            else break;
        }
    }

    void InitializePool(int size)
    {
        for (int i = 0; i < size; i++)
        {
            GameObject note = Instantiate(_notePrefab, transform);
            note.SetActive(false);
            _notePool.Enqueue(note);
        }
    }

    void LoadBGMDataFromConductor()
    {
        if (Conductor.Instance == null) return;

        _currentBGMData = Conductor.Instance.CurrentBGMData;
        if (_currentBGMData == null) return;

        _nextNoteIndex = 0;
        ClearAllNotes();
        _currentBGMData.SortNotes();
    }

    void SpawnNote(NoteData noteData)
    {
        GameObject noteObj = GetNoteFromPool();
        Transform spawnPos = (noteData.type == ENoteType.LNote) ? _leftSpawnPoint : _rightSpawnPoint;
        noteObj.transform.position = spawnPos.position;

        Note noteScript = noteObj.GetComponent<Note>();
        if (noteScript != null)
        {
            noteScript.Initialize(noteData.beat, noteData.type, _beatsShownInAdvance, _judgePoint, this);
            _activeNotes.Add(noteScript);
        }
    }

    GameObject GetNoteFromPool()
    {
        if (_notePool.Count > 0)
        {
            GameObject note = _notePool.Dequeue();
            note.SetActive(true);
            return note;
        }
        return Instantiate(_notePrefab, transform);
    }

    public void ReturnNoteToPool(GameObject note)
    {
        note.SetActive(false);
        _notePool.Enqueue(note);
    }

    public void ClearAllNotes()
    {
        foreach (Note note in _activeNotes)
        {
            if (note != null && note.gameObject != null)
                ReturnNoteToPool(note.gameObject);
        }
        _activeNotes.Clear();
    }

    public List<Note> GetActiveNotes()
    {
        _activeNotes.RemoveAll(note => note == null || !note.gameObject.activeSelf);
        return _activeNotes;
    }

    // 즉시 제거하지 않고, 리스트에서만 제거
    public void RemoveNote(Note note)
    {
        _activeNotes.Remove(note);
        // ReturnNoteToPool는 호출하지 않음!
        // Note 자체가 애니메이션 후 SetActive(false) 처리
    }

    // 애니메이션 후 풀 반환용 (Note에서 호출)
    public void ReturnNoteAfterAnimation(GameObject note)
    {
        ReturnNoteToPool(note);
    }

    public void ReloadBGMData() => LoadBGMDataFromConductor();
}
