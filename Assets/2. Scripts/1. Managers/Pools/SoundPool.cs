using UnityEngine;

public enum ESoundObject
{ 
    SoundObject
}

public class SoundPool : PoolBase<ESoundObject>
{
    private static SoundPool _instance;
    public static SoundPool Instance
    {
        get
        {
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
