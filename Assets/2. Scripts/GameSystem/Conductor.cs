using UnityEngine;

public class Conductor : SimpleSingleton<Conductor>
{
    [Header("BGM 리스트 (5개 곡)")]
    [SerializeField] private BGMDataSO[] _bgmList = new BGMDataSO[5];
    [SerializeField] private int _currentBGMIndex = 0;

    [Header("현재 BGM 상태 (자동 할당)")]
    [SerializeField] private AudioSource _musicSource;
    [Header("싱크 안맞을 때 추가 (초단위 Offset)")]
    [SerializeField] private float _timingOffset = 0f; // 타이밍 오프셋 (초)

    // === Private 변수 ===
    private BGMDataSO _currentBGMData;
    private float _currentBpm;

    // === Public 접근자 ===
    public float SecPerBeat { get; private set; }
    public float BgmPosition { get; private set; }
    public float BgmPositionInBeats { get; private set; }
    public BGMDataSO CurrentBGMData => _currentBGMData;
    public float CurrentBpm => _currentBpm;
    public int CurrentBGMIndex => _currentBGMIndex;
    public BGMDataSO[] BgmList => _bgmList;

    private double _dspBGMTime;
    private bool _isPlaying = false;

    void Start()
    {
        GetBGMSource();

        // 기본 곡 로드 (인덱스 0)
        if (_bgmList.Length > 0 && _bgmList[_currentBGMIndex] != null)
        {
            LoadBGM(_currentBGMIndex);
        }
    }
    void Update()
    {
        if (_isPlaying && _musicSource != null && _musicSource.isPlaying)
        {
            UpdateTiming_DspTime();
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
    // dspTime 기반 타이밍 계산 
    void UpdateTiming_DspTime()
    {
        // 경과 시간 (초)
        BgmPosition = (float)(AudioSettings.dspTime - _dspBGMTime) + _timingOffset;
        // 비트 계산
        BgmPositionInBeats = BgmPosition / SecPerBeat;
    }

    // 인덱스로 BGM 로드
    public void LoadBGM(int index)
    {
        if (index < 0 || index >= _bgmList.Length)
        {
            Debug.LogError($"BGM 인덱스 범위 오류: {index}");
            return;
        }

        BGMDataSO bgmData = _bgmList[index];

        if (bgmData == null || !bgmData.IsValid())
        {
            Debug.LogError($"BGM[{index}]가 유효하지 않습니다!");
            return;
        }

        _currentBGMIndex = index;
        _currentBGMData = bgmData;
        _currentBpm = bgmData.Bpm;
        _musicSource.clip = bgmData.AudioClip;

        Debug.Log($"BGM 로드: [{index}] {bgmData.BgmName}, BPM={_currentBpm}");
    }

    // BGM 재생 시작
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

        if (_currentBGMData.StartDelay > 0f)
        {
            double startTime = AudioSettings.dspTime + _currentBGMData.StartDelay;
            _dspBGMTime = startTime;
            _musicSource.PlayScheduled(startTime);
        }
        else
        {
            _dspBGMTime = AudioSettings.dspTime;
            _musicSource.Play();
        }

        _isPlaying = true;
        Debug.Log($"BGM 재생: {_currentBGMData.BgmName}");
    }

    public void PlayBGM(int index)
    {
        StopBGM();
        LoadBGM(index);

        // NoteSpawner에 알림
        NotifyBGMChanged();

        PlayBGM();
    }

    // 다음 곡
    public void NextBGM()
    {
        int nextIndex = (_currentBGMIndex + 1) % _bgmList.Length;
        PlayBGM(nextIndex);
    }

    // 이전 곡
    public void PreviousBGM()
    {
        int prevIndex = (_currentBGMIndex - 1 + _bgmList.Length) % _bgmList.Length;
        PlayBGM(prevIndex);
    }

    // NoteSpawner에 BGM 변경 알림
    void NotifyBGMChanged()
    {
        NoteSpawner spawner = FindObjectOfType<NoteSpawner>();
        if (spawner != null)
        {
            spawner.ReloadBGMData();
        }
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
    }

    public void PauseBGM()
    {
        _isPlaying = false;
        if (_musicSource != null)
        {
            _musicSource.Pause();
        }
    }

    public void ResumeBGM()
    {
        _isPlaying = true;
        if (_musicSource != null)
        {
            _musicSource.UnPause();
        }
    }

    public bool IsPlaying()
    {
        return _isPlaying && _musicSource != null && _musicSource.isPlaying;
    }

    // BGM 정보 가져오기
    public BGMDataSO GetBGMData(int index)
    {
        if (index >= 0 && index < _bgmList.Length)
        {
            return _bgmList[index];
        }
        return null;
    }
}
