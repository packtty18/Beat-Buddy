using UnityEngine;
public class SoundManager : SimpleSingleton<SoundManager>
{
    /*
     * 게임 내에 생성되는 소리 관련 모든 객체를 동작 및 제어
     */
    private const string _bgmObjectName = "BGM";

    [Header("사운드 프리팹")]
    [SerializeField] private GameObject _soundPrefab;   //사운드 오브젝트
    [SerializeField] private Transform _soundParent;

    [Header("정적 생성된 AudioSource")]
    [SerializeField] private AudioSource _bgmSource;

    [Header("사용할 클립")]
    [SerializeField] private AudioClip _bgmSound;
    [SerializeField] private AudioClip _gameOverSound;
    [SerializeField] private AudioClip _bulletSound;
    [SerializeField] private AudioClip _expSound;
    [SerializeField] private AudioClip _playerHitSound;
    [SerializeField] private AudioClip _chestOpen;
    [SerializeField] private AudioClip _explosion;
    private void Start()
    {
        if (_bgmSource == null)
        {
            CreateBGMObject();
        }
    }

    private void CreateBGMObject()
    {
        GameObject bgmObject = InstantiateSoundObject(_bgmSound, Vector3.zero, _soundParent, false);
        bgmObject.name = _bgmObjectName;
    }

    private GameObject InstantiateSoundObject(AudioClip clip, Vector3 position, Transform parent = null, bool autoDestory = true )
    {
        GameObject soundObject = Instantiate(_soundPrefab, position, Quaternion.identity);
        soundObject.transform.SetParent(parent);

        SoundObject sound = soundObject.GetComponent<SoundObject>();
        sound.SetSound(clip, autoDestory);
        sound.OnPlay();

        return soundObject;
    }
    
    public GameObject CreateSFX(ESFXType sfx, Vector3 position, Transform parent = null)
    {
        AudioClip clipToPlay = null;
        bool autoDestroy = true;

        switch (sfx)
        {
           
                
        }

        GameObject soundGameObject = InstantiateSoundObject(clipToPlay, position, parent, autoDestroy);
        SoundObject soundObject = soundGameObject.GetComponent<SoundObject>();
        soundObject.OnPlay();

        return soundGameObject;
    }
}

