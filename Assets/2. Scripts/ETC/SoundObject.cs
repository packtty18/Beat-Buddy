using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundObject : MonoBehaviour, IPoolable
{
    private AudioSource _audio;
    private Coroutine _lifetimeRoutine;

    public Action OnPlaybackFinished;


    private void Awake()
    {
        _audio = GetComponent<AudioSource>();
    }

    private void StopLifetimeRoutine()
    {
        if (_lifetimeRoutine != null)
        {
            StopCoroutine(_lifetimeRoutine);
            _lifetimeRoutine = null;
        }
    }

    public void OnSpawn()
    {
        StopLifetimeRoutine();
    }

    public void OnDespawn()
    {
        StopLifetimeRoutine();
    }

    public void SetVolume(float volume)
    {
        _audio.volume = volume;
    }

    public void OnPlay(AudioClip clip, bool isBgm, float volume, double delayTime = -1)
    {
        StopLifetimeRoutine();

        if (clip == null)
        {
            Debug.LogWarning("[SoundObject] OnPlay called with null clip.");
            return;
        }

        _audio.clip = clip;
        _audio.loop = isBgm;
        _audio.volume = volume;

        if(delayTime < 0)
        {
            _audio.Play();
        }
        else
        {
            _audio.PlayScheduled(delayTime);
        }
        
    }

    


    public void StopPlay()
    {
        _audio.Stop();
    }

    private void DespawnObject()
    {
        OnPlaybackFinished?.Invoke();
    }
}
