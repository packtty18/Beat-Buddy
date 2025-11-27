using UnityEngine;

public class TestSound : TestBase
{
    [Header("Debug Zone")]
    [SerializeField]
    private ESoundType _debugSelectedType = ESoundType.None;
    [SerializeField]
    private bool _debugIsBGM = false;


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
            sound.PlayBGM(_debugSelectedType);
        }
        else
        {
            sound.PlaySFX(_debugSelectedType);
        }
    }
}
