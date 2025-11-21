using System;
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

    public void OnPlay(AudioClip clip, bool isBgm, float playTime = 0f)
    {
        if (clip == null)
        {
            Debug.LogWarning("[SoundObject] OnPlay called with null clip.");
            return;
        }

        _audio.clip = clip;
        _audio.loop = isBgm;
        _audio.Play();

        float duration = GetPlayDuration(clip, isBgm, playTime);

        if (duration > 0f)
        {
            StartCoroutine(Wait(duration, isBgm ? StopPlay : DespawnObject));
        }
    }

    private float GetPlayDuration(AudioClip clip, bool isBgm, float playTime)
    {
        if (playTime > 0f)
        {
            return isBgm ? playTime : Mathf.Min(playTime, clip.length);
        }

        return isBgm ? 0f : clip.length; // BGM은 loop이므로 Wait 필요 없음
    }

    private IEnumerator Wait(float duration, Action callback)
    {
        yield return new WaitForSeconds(duration);
        callback?.Invoke();
    }


    public void StopPlay()
    {
        _audio.Stop();
    }


    private void DespawnObject()
    {
        PoolManager.Instance.Despawn<SoundPool, ESoundObject>(ESoundObject.SoundObject, gameObject);
    }
}
