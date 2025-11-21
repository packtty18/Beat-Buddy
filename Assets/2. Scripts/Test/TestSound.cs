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
        if(!SoundManager.IsManagerExist())
        {
            return;
        }
        SoundManager sound = SoundManager.Instance;
        if(_debugIsBGM)
        {
            sound.PlayBGM(_debugSelectedType, _debugPlayTime);
        }
        else
        {
            sound.PlaySFX(_debugSelectedType, _debugPlayTime);
        }
    }
}
