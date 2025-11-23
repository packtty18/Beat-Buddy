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
}

[CreateAssetMenu(fileName = "TransitionDatabase", menuName = "SO/TransitionDatabase")]
public class TransitionDatabaseSO : DatabaseSO<ETransitionType, TransitionBase> { }