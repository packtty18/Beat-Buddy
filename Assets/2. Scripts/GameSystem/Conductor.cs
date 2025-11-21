using UnityEngine;

public class Conductor : SimpleSingleton<Conductor>
{
    [Header("BGM 데이터")]
    [SerializeField] private BGMDataSO _currentBGMData;

    [Header("현재 BGM 상태 (자동 할당)")]
    [SerializeField] private AudioSource _musicSource;
    [SerializeField] private float _currentBpm = 120f;


    public float SecPerBeat { get; private set; }
    public float BgmPosition { get; private set; }
    public float BgmPositionInBeats { get; private set; }

    private double _dspBGMTime;
    private bool _isPlaying = false;

    public BGMDataSO CurrentBGMData => _currentBGMData;
    public float CurrentBpm => _currentBpm;

    void Start()
    {
        GetBGMSource();

        if (_currentBGMData != null)
        {
            LoadBGMData(_currentBGMData);
        }
    }

    void Update()
    {
        if (_isPlaying && _musicSource != null && _musicSource.isPlaying)
        {
            UpdateTiming();
        }
    }

    void GetBGMSource()
    {
        _musicSource = GetComponent<AudioSource>();
        if (_musicSource == null)
        {
            _musicSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void UpdateTiming()
    {
        BgmPosition = (float)(AudioSettings.dspTime - _dspBGMTime);
        BgmPositionInBeats = BgmPosition / SecPerBeat;
    }

    /// <summary>
    /// BGMDataSO 로드 (AudioClip 자동 할당)
    /// </summary>
    void LoadBGMData(BGMDataSO bgmData)
    {
        if (bgmData == null || !bgmData.IsValid())
        {
            Debug.LogError("유효하지 않은 BGMDataSO!");
            return;
        }

        _currentBGMData = bgmData;
        _currentBpm = bgmData.Bpm;

        // AudioClip 자동 할당
        _musicSource.clip = bgmData.AudioClip;

        Debug.Log($"BGM 로드: {bgmData.BgmName}, BPM={_currentBpm}, 길이={bgmData.GetDuration():F2}초");
    }

    /// <summary>
    /// BGM 재생 시작
    /// </summary>
    public void PlayBGM()
    {
        if (_currentBGMData == null)
        {
            Debug.LogError("BGMDataSO가 설정되지 않았습니다!");
            return;
        }

        if (_musicSource.clip == null)
        {
            Debug.LogError("AudioClip이 없습니다!");
            return;
        }

        _currentBpm = _currentBGMData.Bpm;
        SecPerBeat = 60f / _currentBpm;

        // StartDelay 처리
        if (_currentBGMData.StartDelay > 0f)
        {
            double startTime = AudioSettings.dspTime + _currentBGMData.StartDelay;
            _dspBGMTime = startTime;
            _musicSource.PlayScheduled(startTime);
            Debug.Log($"BGM 재생 예약: {_currentBGMData.StartDelay}초 후");
        }
        else
        {
            _dspBGMTime = AudioSettings.dspTime;
            _musicSource.Play();
        }

        _isPlaying = true;
        Debug.Log($"BGM 재생 시작: {_currentBGMData.BgmName}, BPM={_currentBpm}");
    }

    /// <summary>
    /// BGMDataSO 변경 (곡 전환)
    /// </summary>
    public void SetBGMData(BGMDataSO bgmData)
    {
        StopBGM();  // 기존 BGM 정지
        LoadBGMData(bgmData);
    }

    public void StopBGM()
    {
        _isPlaying = false;
        BgmPosition = 0f;
        BgmPositionInBeats = 0f;

        if (_musicSource != null)
        {
            _musicSource.Stop();
        }

        Debug.Log("BGM 정지");
    }

    public void PauseBGM()
    {
        _isPlaying = false;

        if (_musicSource != null)
        {
            _musicSource.Pause();
        }

        Debug.Log("BGM 일시정지");
    }

    public void ResumeBGM()
    {
        _isPlaying = true;

        if (_musicSource != null)
        {
            _musicSource.UnPause();
        }

        Debug.Log("BGM 재개");
    }

    public bool IsPlaying()
    {
        return _isPlaying && _musicSource != null && _musicSource.isPlaying;
    }
}
