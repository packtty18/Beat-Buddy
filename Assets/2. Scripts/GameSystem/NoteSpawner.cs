using System.Collections.Generic;
using UnityEditor.EditorTools;
using UnityEngine;

public class NoteSpawner : MonoBehaviour
{
    [Header("노트 설정")]
    [SerializeField] private Transform _leftSpawnPoint;
    [SerializeField] private Transform _rightSpawnPoint;
    [SerializeField] private Transform _judgePoint;
    [SerializeField] private float _beatsShownInAdvance = 5f;

    private BGMDataSO _currentBGMData;
    private int _nextNoteIndex = 0;
    private List<Note> _activeNotes = new List<Note>();
    private PoolManager _poolManager;
    void Start()
    {
        _poolManager = PoolManager.Instance;
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
        Note noteObject = _poolManager.SpawnGetComponent<NotePool, ENoteType, Note>(noteData.type);

        if (noteObject == null) return;

        Transform spawnPos = (noteData.type == ENoteType.LNote) ? _leftSpawnPoint : _rightSpawnPoint;
        noteObject.transform.position = spawnPos.position;



        noteObject.Initialize(noteData.beat, noteData.type, _beatsShownInAdvance, _judgePoint, this);

        _activeNotes.Add(noteObject);
    }

    public void ReturnNoteToPool(GameObject note)
    {
        note.SetActive(false);
        _poolManager.Despawn<NotePool, ENoteType>(note.GetComponent<Note>().NoteType, note);
    }

    public void ClearAllNotes()
    {
        foreach (Note note in _activeNotes)
        {
            if (note != null && note.gameObject != null)
            {
                _poolManager.Despawn<NotePool, ENoteType>(note.GetComponent<Note>().NoteType, note.gameObject);
            }
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
    }

    // 애니메이션 후 풀 반환용 (Note에서 호출)
    public void ReturnNoteAfterAnimation(GameObject note)
    {
        _poolManager.Despawn<NotePool, ENoteType>(note.GetComponent<Note>().NoteType, note);
    }

    public void ReloadBGMData() => LoadBGMDataFromConductor();
}
