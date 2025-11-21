using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundObject : MonoBehaviour, IPoolable
{
    private AudioSource _audio;
    private Coroutine _lifetimeRoutine;

    private void Awake()
    {
        _audio = GetComponent<AudioSource>();
    }

    public void OnSpawn()
    {
        if (_lifetimeRoutine != null)
        {
            StopCoroutine(_lifetimeRoutine);
            _lifetimeRoutine = null;
        }
    }

    public void OnDespawn()
    {
        if (_lifetimeRoutine != null)
        {
            StopCoroutine(_lifetimeRoutine);
            _lifetimeRoutine = null;
        }

        if (_audio.isPlaying)
        {
            StopPlay();
        }
    }

    public void OnPlay(AudioClip clip, bool isBgm, float playTime = 0)
    {
        _audio.clip = clip;
        _audio.loop = isBgm;
        _audio.Play();
        if (!isBgm)
        {
            StartCoroutine(AutoDespawn(clip.length));

        }
        else if (playTime > 0)
        {
            StartCoroutine(AutoDespawn(playTime));
        }
    }

    public void StopPlay(bool autoDespawn = false)
    {
        _audio.Stop();

        if(autoDespawn)
        {
            PoolManager.Instance.Despawn<SoundPool, ESoundObject>(ESoundObject.SoundObject, gameObject);
        }
    }


    private IEnumerator AutoDespawn(float delay)
    {
        yield return new WaitForSeconds(Mathf.Max(0f, delay));
        PoolManager.Instance.Despawn<SoundPool, ESoundObject>(ESoundObject.SoundObject, gameObject);
    }
}
