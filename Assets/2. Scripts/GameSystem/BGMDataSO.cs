using UnityEngine;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "NewBGMData", menuName = "Rhythm/BGM Data SO")]
public class BGMDataSO : ScriptableObject
{
    [Header("BGM 정보")]
    [SerializeField] private string _bgmName;
    [SerializeField] private float _bpm;
    [SerializeField] private AudioClip _audioClip;

    [Header("노트 데이터")]
    [SerializeField] private NoteData[] _notes = new NoteData[0];

    [Header("난이도")]
    [Range(1, 5)]
    [SerializeField] private int _difficulty = 3;

    public string BgmName => _bgmName;
    public float Bpm => _bpm;
    public AudioClip AudioClip => _audioClip;
    public NoteData[] Notes => _notes;
    public int Difficulty => _difficulty;

    public float GetDuration() => _audioClip != null ? _audioClip.length : 0f;

    public bool IsValid()
    {
        if (_audioClip == null)
        {
            Debug.LogError($"[{_bgmName}]: AudioClip이 없습니다!");
            return false;
        }
        return true;
    }

    [ContextMenu("BPM 자동 분석")]
    public void AnalyzeBPM()
    {
        if (_audioClip == null)
        {
            Debug.LogError("AudioClip이 없습니다!");
            return;
        }

        _bpm = UniBpmAnalyzer.AnalyzeBpm(_audioClip);

#if UNITY_EDITOR
        EditorUtility.SetDirty(this);
#endif
    }

    [ContextMenu("노트 정렬")]
    public void SortNotes()
    {
        System.Array.Sort(_notes, (a, b) => a.beat.CompareTo(b.beat));

#if UNITY_EDITOR
        EditorUtility.SetDirty(this);
#endif
    }

#if UNITY_EDITOR
    [ContextMenu("OSU 파일에서 임포트")]
    public void ImportFromOsuFile()
    {
        string path = EditorUtility.OpenFilePanel("OSU 파일 선택", Application.dataPath, "osu");
        if (string.IsNullOrEmpty(path)) return;

        var osuData = OsuParser.ParseOsuFile(path);
        if (osuData == null)
        {
            Debug.LogError("OSU 파일 파싱 실패!");
            return;
        }

        _bpm = osuData.bpm;

        if (string.IsNullOrEmpty(_bgmName))
            _bgmName = string.IsNullOrEmpty(osuData.title) ? "Imported Song" : osuData.title;

        _notes = osuData.notes.ToArray();
        SortNotes();

        Debug.Log($"=== OSU 임포트 완료 ===");
        Debug.Log($"곡명: {_bgmName}");
        Debug.Log($"BPM: {_bpm:F2}");
        Debug.Log($"노트 수: {_notes.Length}개");
        Debug.Log($"오디오 파일: {osuData.audioFileName}");
        Debug.Log("AudioClip을 수동으로 할당해주세요!");

        EditorUtility.SetDirty(this);
    }
#endif

    public int GetTotalNotes() => _notes.Length;

    public int GetLNoteCount()
    {
        return _notes.Count(note => note.type == ENoteType.LNote);
    }

    public int GetRNoteCount()
    {
        return _notes.Count(note => note.type == ENoteType.RNote);
    }
}
