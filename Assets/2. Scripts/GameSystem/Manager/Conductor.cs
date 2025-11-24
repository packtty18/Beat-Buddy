using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Conductor : SimpleSingleton<Conductor>
{
    [Header("BGM 설정")]
    [SerializeField] private BGMDataSO[] _bgmList = new BGMDataSO[5];
    [SerializeField] private int _currentBGMIndex = 0;
    [SerializeField] private float _readyTime = 3f;

    private AudioSource _musicSource;
    private BGMDataSO _currentBGMData;
    private double _dspBGMTime;
    private double _songEndTime; // 음악 종료 시간 추적
    private bool _isPlaying = false;
    private bool _isReady = false;

    public float SecPerBeat { get; private set; }
    public float BgmPosition { get; private set; }
    public float BgmPositionInBeats { get; private set; }
    public BGMDataSO CurrentBGMData => _currentBGMData;
    public float CurrentBpm => _currentBGMData?.Bpm ?? 0f;
    public bool IsReady => _isReady;

    protected override void Awake()
    {
        base.Awake();
        _musicSource = GetComponent<AudioSource>();
        _musicSource.playOnAwake = false;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.StopBGM();
        }

        if (_bgmList.Length > 0 && _bgmList[_currentBGMIndex] != null)
        {
            LoadBGM(_currentBGMIndex);
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
            _isReady = true;
        }
        // 음악 재생 중
        else if (currentDspTime < _songEndTime)
        {
            BgmPosition = (float)(currentDspTime - _dspBGMTime);
            BgmPositionInBeats = BgmPosition / SecPerBeat;
            _isReady = false;
        }
        // 음악 종료
        else
        {
            BgmPosition = (float)(_songEndTime - _dspBGMTime);
            BgmPositionInBeats = BgmPosition / SecPerBeat;
            _isPlaying = false;
            _isReady = false;

            Debug.Log($"[Conductor] 음악 재생 완료 - 총 재생 시간: {BgmPosition:F2}초");
        }
    }

    public void LoadBGM(int index)
    {
        if (index < 0 || index >= _bgmList.Length || _bgmList[index] == null)
        {
            Debug.LogWarning($"[Conductor] 유효하지 않은 BGM 인덱스: {index}");
            return;
        }

        _currentBGMIndex = index;
        _currentBGMData = _bgmList[index];
        SecPerBeat = 60f / _currentBGMData.Bpm;

        Debug.Log($"[Conductor] BGM 로드: {_currentBGMData.BgmName}, BPM: {_currentBGMData.Bpm}");
    }

    public void PlayBGM()
    {
        if (_currentBGMData == null || _musicSource == null)
        {
            Debug.LogError("[Conductor] BGM 데이터 또는 AudioSource가 없습니다!");
            return;
        }

        if (_currentBGMData.AudioClip == null)
        {
            Debug.LogError("[Conductor] AudioClip이 없습니다!");
            return;
        }

        _musicSource.clip = _currentBGMData.AudioClip;
        SecPerBeat = 60f / _currentBGMData.Bpm;

        // 재생 시작 시간과 종료 시간 계산
        _dspBGMTime = AudioSettings.dspTime + _readyTime;
        _songEndTime = _dspBGMTime + _musicSource.clip.length;

        _musicSource.PlayScheduled(_dspBGMTime);
        _isReady = true;
        _isPlaying = true;

        Debug.Log($"[Conductor] BGM 재생 예약 - 곡 길이: {_musicSource.clip.length:F2}초, 시작 시간: {_dspBGMTime:F2}, 종료 시간: {_songEndTime:F2}");
    }

    public void StopBGM()
    {
        if (_musicSource != null)
        {
            _musicSource.Stop();
            _isPlaying = false;
            _isReady = false;
            Debug.Log("[Conductor] BGM 정지");
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

    private void OnDestroy()
    {
        StopBGM();
    }
}
