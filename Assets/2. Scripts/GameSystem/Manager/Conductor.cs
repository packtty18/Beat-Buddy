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
    }

    private void Start()
    {
        SoundManager.Instance.StopBGM();
        if (_bgmList.Length > 0 && _bgmList[_currentBGMIndex] != null)
        {
            LoadBGM(_currentBGMIndex);
        }
    }

    private void Update()
    {
        if (!_isPlaying || _musicSource == null) return;

        double currentDspTime = AudioSettings.dspTime;

        if (currentDspTime < _dspBGMTime)
        {
            float timeUntilStart = (float)(_dspBGMTime - currentDspTime);
            BgmPosition = -timeUntilStart;
            BgmPositionInBeats = BgmPosition / SecPerBeat;
        }
        else if (_musicSource.isPlaying)
        {
            BgmPosition = (float)(currentDspTime - _dspBGMTime);
            BgmPositionInBeats = BgmPosition / SecPerBeat;
            _isReady = false;
        }
        else
        {
            _isPlaying = false;
        }
    }

    public void LoadBGM(int index)
    {
        if (index < 0 || index >= _bgmList.Length || _bgmList[index] == null) return;

        _currentBGMIndex = index;
        _currentBGMData = _bgmList[index];
        SecPerBeat = 60f / _currentBGMData.Bpm;
    }

    public void PlayBGM()
    {
        if (_currentBGMData == null || _musicSource == null) return;

        _musicSource.clip = _currentBGMData.AudioClip;
        SecPerBeat = 60f / _currentBGMData.Bpm;

        _dspBGMTime = AudioSettings.dspTime + _readyTime;
        _musicSource.PlayScheduled(_dspBGMTime);
        _isReady = true;
        _isPlaying = true;
    }

    public void StopBGM()
    {
        if (_musicSource != null)
        {
            _musicSource.Stop();
            _isPlaying = false;
            _isReady = false;
        }
    }

    public bool IsPlaying() => _isPlaying && _musicSource != null && _musicSource.isPlaying;
}
