using UnityEngine;

public class TestSound : MonoBehaviour
{
    [Header("Debug Zone")]
    [SerializeField]
    private ESoundType _debugSelectedType = ESoundType.None;
    [SerializeField]
    private bool _debugIsBGM = false;
    [SerializeField]
    private float _debugPlayTime = 0;


    [ContextMenu("Play Debug Sound")]
    public void PlayDebugSound()
    {
        SoundLibrarySO library = SoundManager.Instance.GetLibrary();
        if (library == null)
        {
            Debug.LogWarning("[SoundManager Debug] SoundLibrary is missing!");
            return;
        }

        if (_debugSelectedType == ESoundType.None)
        {
            Debug.LogWarning("[SoundManager Debug] No sound type selected!");
            return;
        }

        AudioClip clip = library.GetClip(_debugSelectedType);
        if (clip == null)
        {
            Debug.LogWarning($"[SoundManager Debug] Clip not found for {_debugSelectedType}");
            return;
        }

        if (_debugIsBGM)
        {
            SoundObject bgmSource = SoundManager.Instance.GetBgmSource();
            // BGM용 사운드 오브젝트 생성 혹은 변경
            if (bgmSource == null)
            {
                SoundManager.Instance.Init();
            }
            bgmSource.OnPlay(clip, true, _debugPlayTime);
        }
        else
        {
            GameObject soundObject = PoolManager.Instance.Spawn<SoundPool, ESoundObject>(ESoundObject.SoundObject);
            var so = soundObject.GetComponent<SoundObject>();
            so.OnPlay(clip, false, _debugPlayTime);
        }
    }
}
