using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SoundEntry
{
    public ESoundType Type;
    public AudioClip Clip;
}

[CreateAssetMenu(fileName = "SoundLibrary", menuName = "SO/SoundLibrary")]
public class SoundLibrarySO : ScriptableObject
{
    [SerializeField] private List<SoundEntry> _soundList = new List<SoundEntry>();
    private Dictionary<ESoundType, AudioClip> _soundMap;

    public void InitSoundMap()
    {
        if (_soundMap != null)
        {
            return;
        }

        _soundMap = new Dictionary<ESoundType, AudioClip>();
        foreach (SoundEntry sound in _soundList)
        {
            if (sound == null)
            {
                continue;
            }

            if (sound.Type == ESoundType.None)
            {
                continue;
            }

            if (!_soundMap.ContainsKey(sound.Type))
            {
                _soundMap.Add(sound.Type, sound.Clip);
            }
            else
            {
                Debug.LogWarning($"SoundLibrary: Duplicate entry for {sound.Type}");
            }
        }
    }

    public AudioClip GetClip(ESoundType type)
    {
        _soundMap.TryGetValue(type, out AudioClip clip);
        return clip;
    }
}
