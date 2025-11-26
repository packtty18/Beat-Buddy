using UnityEngine;

public enum ETransitionType
{
    None,
    FadeOut,
    FadeIn,
    ScaleOut,
    ScaleIn,
    SlideOut,
    SlideIn,
    SlideStageIn,
}

[CreateAssetMenu(fileName = "TransitionDatabase", menuName = "SO/TransitionDatabase")]
public class TransitionDatabaseSO : DatabaseSO<ETransitionType, TransitionBase> 
{
}
