using UnityEngine;

public enum ESFXType
{
    HitDrum,
    HitClap,
}

public class AudioManager : SimpleSingleton<AudioManager>
{
    [Header("리듬게임 키 입력음")]
    [SerializeField] private AudioClip _hitDrumSound;
    [SerializeField] private AudioClip _hitClapSound;
    [SerializeField] private float _volume = 1f;

    private AudioSource _sfxSource;

    protected override void Awake()
    {
        base.Awake();

        _sfxSource = gameObject.AddComponent<AudioSource>();
        _sfxSource.playOnAwake = false;
        _sfxSource.volume = _volume;
    }

    void Start()
    {
        CheckAudioClip("HitDrum", _hitDrumSound);
        CheckAudioClip("HitClap", _hitClapSound);
    }

    void CheckAudioClip(string name, AudioClip clip)
    {
        if (clip != null)
        {
            Debug.Log($"{name}: {clip.length}초, {clip.frequency}Hz");
        }
        else
        {
            Debug.LogError($"{name}: 클립 없음!");
        }
    }

    public void CreateSFX(ESFXType sfx, Vector3 position)
    {
        AudioClip clipToPlay = sfx switch
        {
            ESFXType.HitDrum => _hitDrumSound,
            ESFXType.HitClap => _hitClapSound,
            _ => null
        };

        if (clipToPlay == null)
        {
            Debug.LogError($"AudioClip 없음: {sfx}");
            return;
        }

        _sfxSource.PlayOneShot(clipToPlay, _volume);
        Debug.Log($"재생: {clipToPlay.name}");
    }
}