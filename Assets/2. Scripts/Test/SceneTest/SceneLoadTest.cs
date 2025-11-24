using UnityEngine;

public class SceneLoadTest : MonoBehaviour
{
    [SerializeField] private ESceneType _targetScene = ESceneType.None;
    [SerializeField] private ETransitionType _outTransitionType = ETransitionType.None;
    [SerializeField] private ETransitionType _inTransitionType = ETransitionType.None;
    [SerializeField] private bool _blockLoadSameScene = true;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    public void TestSceneConvert()
    {
        MySceneManager.Instance.LoadScene(_targetScene, _outTransitionType, _inTransitionType, _blockLoadSameScene);
    }
}
