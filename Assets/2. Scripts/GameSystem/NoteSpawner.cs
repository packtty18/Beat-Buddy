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
    private bool _isSpawningEnabled = false;

    void Start()
    {
        _poolManager = PoolManager.Instance;
        LoadBGMDataFromSongPlayManager();
    }

    void Update()
    {
        if (!_isSpawningEnabled) return;

        if (_currentBGMData == null || _nextNoteIndex >= _currentBGMData.Notes.Length) return;

        
        float currentBeat = SongPlayManager.Instance.BgmPositionInBeats;

        while (_nextNoteIndex < _currentBGMData.Notes.Length)
        {
            NoteData nextNote = _currentBGMData.Notes[_nextNoteIndex];
            // Max 때문에 이전에 스폰되는 노트 모두 삭제됐었음.
            float spawnBeat = nextNote.beat - _beatsShownInAdvance; 

            if (currentBeat >= spawnBeat)
            {
                SpawnNote(nextNote);
                _nextNoteIndex++;
            }
            else break;
        }
    }

    void LoadBGMDataFromSongPlayManager()
    {
        if (SongPlayManager.Instance == null) return;

        _currentBGMData = SongPlayManager.Instance.CurrentBGMData;
        if (_currentBGMData == null) return;

        _nextNoteIndex = 0;
        ClearAllNotes();
        _currentBGMData.SortNotes();
    }

    public void StartSpawning()
    {
        _nextNoteIndex = 0;
        _isSpawningEnabled = true;
        Debug.Log("[NoteSpawner] 노트 스폰 시작!");
    }

    public void StopSpawning()
    {
        _isSpawningEnabled = false;
        Debug.Log("[NoteSpawner] 노트 스폰 중지!");
    }

    public void ChangeSpawnerPosition()
    {
        Transform tempSpawnPoint = _leftSpawnPoint;
        _leftSpawnPoint = _rightSpawnPoint;
        _rightSpawnPoint = tempSpawnPoint;
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

    public bool IsSpawningEnabled()
    {
        return _isSpawningEnabled;
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

    public void ReloadBGMData() => LoadBGMDataFromSongPlayManager();
}
