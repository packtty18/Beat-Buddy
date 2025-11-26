using UnityEngine;

//씬이 변경되도 파괴되지 않음
public class CoreSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;
    public static T Instance
    {
        get
        {
            return _instance;
        }
    }

    protected virtual void Awake()
    {
        if (_instance == null)
        {
            _instance = this as T;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static bool IsManagerExist()
    {
        if(_instance == null)
        {
            Debug.LogWarning($"{typeof(T).Name} is not Exist");
        }

        return _instance != null;
    }
}
