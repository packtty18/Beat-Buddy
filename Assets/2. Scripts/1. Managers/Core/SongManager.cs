using UnityEngine;

public enum ESongType
{
    None = 0,
    FlameTheme = 1,
    WaterTheme = 2,
    ThunderTheme = 3,
    EsperTheme = 4,
    FinalTheme = 5,
    KonTheme,
}

public class SongManager : CoreSingleton<SongManager>
{
    [Header("곡 데이터베이스")]
    [SerializeField] private SongDatabaseSO _songDatabase;

    private BGMDataSO _selectedSong;
    private ESongType _selectedSongType = ESongType.None;

    // 프로퍼티
    public BGMDataSO SelectedSong => _selectedSong;
    public ESongType SelectedSongType => _selectedSongType;
    public int SongCount => _songDatabase?.Count ?? 0;

    protected override void Awake()
    {
        base.Awake();

        if (_songDatabase != null)
        {
            _songDatabase.InitMap();
            Debug.Log($"[SongManager] 곡 데이터베이스 초기화 완료: {_songDatabase.Count}곡");
        }
        else
        {
            Debug.LogError("[SongManager] SongDatabase가 할당되지 않았습니다!");
        }
    }

    // 인덱스로 곡 선택 (UI 호환성 유지)
    public void SelectSongByIndex(int index)
    {
        if (_songDatabase == null)
        {
            Debug.LogError("[SongManager] SongDatabase가 없습니다!");
            return;
        }

        var allSongs = _songDatabase.GetAllData();
        if (index >= 0 && index < allSongs.Count)
        {
            BGMDataSO song = allSongs[index];
            _selectedSong = song;
            _selectedSongType = song.SongType;
        }
        else
        {
            Debug.LogError($"[SongManager] 유효하지 않은 곡 인덱스: {index}");
        }
    }

    // ESongType으로 곡 가져오기 (SongPlayManager)
    public BGMDataSO GetSong(ESongType songType)
    {
        if (_songDatabase == null)
        {
            Debug.LogError("[SongManager] SongDatabase가 없습니다!");
            return null;
        }

        return _songDatabase.GetData(songType);
    }

    // 모든 곡 가져오기 (UI용)
    public BGMDataSO[] GetAllSongs()
    {
        if (_songDatabase == null)
        {
            Debug.LogError("[SongManager] SongDatabase가 없습니다!");
            return new BGMDataSO[0];
        }
        return _songDatabase.GetAllData().ToArray();
    }
    public string GetSelectedSongName()
    {
        return _selectedSong.BgmName;
    }
    // 선택된 곡의 인덱스 가져오기 (UI 호환성)
    public int GetSelectedSongIndex()
    {
        if (_selectedSong == null || _songDatabase == null)
            return 0;

        var allSongs = _songDatabase.GetAllData();
        return allSongs.IndexOf(_selectedSong);
    }
}
