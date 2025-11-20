using UnityEngine;

public class SoundObject : MonoBehaviour
{
    private AudioSource _audio;
    private bool _autoDestroy;

    private void Awake()
    {
        _audio = GetComponent<AudioSource>();
    }

    public void SetSound(AudioClip clip, bool autoDestroy = true)
    {
        _audio.clip = clip;
        _autoDestroy = autoDestroy;
    }

    public void OnPlay()
    {
        _audio.Play();

        if (_autoDestroy)
        {
            AudioClip clip = _audio.clip;
            Destroy(gameObject, clip.length);
        }
        else
        {
            _audio.loop = true;
        }
    }

}
