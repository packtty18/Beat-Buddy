using UnityEngine;

public class NotePool : PoolBase<ENoteType>
{
    private static NotePool _instance;
    public static NotePool Instance
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
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
