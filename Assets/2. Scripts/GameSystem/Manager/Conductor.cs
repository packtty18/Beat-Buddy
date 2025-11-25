using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SongPlayManager : SceneSingleton<SongPlayManager>
{
    [Header("BGM 설정")]
    [SerializeField] private float _readyTime = 3f;

    private AudioSource _musicSource;
    private BGMDataSO _currentBGMData;
    private double _dspBGMTime;
    private double _songEndTime; // 음악 종료 시간 추적
    private bool _isPlaying = false;
    private bool _isSpawnNow = false;

    public float SecPerBeat { get; private set; }
    public float BgmPosition { get; private set; }
    public float BgmPositionInBeats { get; private set; }
    public BGMDataSO CurrentBGMData => _currentBGMData;
    public float CurrentBpm => _currentBGMData?.Bpm ?? 0f;
    public bool IsSpawnNow => _isSpawnNow;

    protected override void Awake()
    {
        base.Awake();
        _musicSource = GetComponent<AudioSource>();
        _musicSource.playOnAwake = false;
    }

    private void Start()
    {
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.StopBGM();
        }
    }

    private void Update()
    {
        if (!_isPlaying || _musicSource == null) return;

        double currentDspTime = AudioSettings.dspTime;

        // 음악 시작 전 (준비 시간)
        if (currentDspTime < _dspBGMTime)
        {
            float timeUntilStart = (float)(_dspBGMTime - currentDspTime);
            BgmPosition = -timeUntilStart;
            BgmPositionInBeats = BgmPosition / SecPerBeat;
            _isSpawnNow = true;
        }
        // 음악 재생 중
        else if (currentDspTime < _songEndTime)
        {
            BgmPosition = (float)(currentDspTime - _dspBGMTime);
            BgmPositionInBeats = BgmPosition / SecPerBeat;
            _isSpawnNow = false;
        }
        // 음악 종료
        else
        {
            BgmPosition = (float)(_songEndTime - _dspBGMTime);
            BgmPositionInBeats = BgmPosition / SecPerBeat;
            _isPlaying = false;
            _isSpawnNow = false;

            Debug.Log($"[SongPlayManager] 음악 재생 완료 - 총 재생 시간: {BgmPosition:F2}초");
        }
    }

    public void LoadSelectedSong()
    {
        if (SongManager.Instance == null)
        {
            Debug.LogError("[SongPlayManager] SongManager가 없습니다!");
            return;
        }

        BGMDataSO selectedSong = SongManager.Instance.SelectedSong;
        if (selectedSong == null)
        {
            Debug.LogError("[SongPlayManager] 선택된 곡이 없습니다!");
            return;
        }

        LoadBGMData(selectedSong);
    }

    public void LoadBGM(ESongType songType)
    {
        if (SongManager.Instance == null)
        {
            Debug.LogError("[SongPlayManager] SongManager가 없습니다!");
            return;
        }

        BGMDataSO song = SongManager.Instance.GetSong(songType);
        if (song == null)
        {
            Debug.LogError($"[SongPlayManager] 곡을 찾을 수 없습니다: {songType}");
            return;
        }

        LoadBGMData(song);
    }

    private void LoadBGMData(BGMDataSO bgmData)
    {
        if (bgmData == null)
        {
            Debug.LogError("[SongPlayManager] BGM 데이터가 null입니다!");
            return;
        }

        if (bgmData.AudioClip == null)
        {
            Debug.LogError($"[SongPlayManager] {bgmData.BgmName}의 AudioClip이 없습니다!");
            return;
        }

        _currentBGMData = bgmData;
        SecPerBeat = 60f / _currentBGMData.Bpm;

        Debug.Log($"[SongPlayManager] BGM 로드: {_currentBGMData.BgmName} ({bgmData.SongType}), BPM: {_currentBGMData.Bpm}");
    }

    public void PlayBGM()
    {
        if (_currentBGMData == null || _musicSource == null)
        {
            Debug.LogError("[SongPlayManager] BGM 데이터 또는 AudioSource가 없습니다!");
            return;
        }

        if (_currentBGMData.AudioClip == null)
        {
            Debug.LogError("[SongPlayManager] AudioClip이 없습니다!");
            return;
        }

        _musicSource.clip = _currentBGMData.AudioClip;
        SecPerBeat = 60f / _currentBGMData.Bpm;

        // 재생 시작 시간과 종료 시간 계산
        _dspBGMTime = AudioSettings.dspTime + _readyTime;
        _songEndTime = _dspBGMTime + _musicSource.clip.length;

        _musicSource.PlayScheduled(_dspBGMTime);
        _isSpawnNow = true;
        _isPlaying = true;

        Debug.Log($"[SongPlayManager] BGM 재생 예약 - 곡 길이: {_musicSource.clip.length:F2}초");
    }

    public void StopBGM()
    {
        if (_musicSource != null)
        {
            _musicSource.Stop();
            _isPlaying = false;
            _isSpawnNow = false;
            Debug.Log("[SongPlayManager] BGM 정지");
        }
    }

    // 음악이 재생 중인지 확인
    public bool IsPlaying()
    {
        return _isPlaying;
    }

    // 남은 재생 시간 반환 (초)
    public float GetRemainingTime()
    {
        if (!_isPlaying) return 0f;

        double currentDspTime = AudioSettings.dspTime;

        // 아직 시작 전
        if (currentDspTime < _dspBGMTime)
        {
            return (float)(_songEndTime - _dspBGMTime);
        }

        // 재생 중
        float remaining = (float)(_songEndTime - currentDspTime);
        return Mathf.Max(0f, remaining);
    }

    // 전체 곡 길이 반환 (초)
    public float GetTotalDuration()
    {
        return _currentBGMData?.AudioClip?.length ?? 0f;
    }

    // 재생 진행도 반환 (0~1)
    public float GetProgress()
    {
        if (!_isPlaying || _currentBGMData == null) return 0f;

        float totalLength = _currentBGMData.AudioClip.length;
        if (totalLength <= 0f) return 0f;

        return Mathf.Clamp01(BgmPosition / totalLength);
    }
}
