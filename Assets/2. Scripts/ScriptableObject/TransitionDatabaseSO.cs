using UnityEngine;

public enum ETransitionType
{
    FadeIn,
    FadeOut,
}

[CreateAssetMenu(fileName = "TransitionDatabase", menuName = "SO/TransitionDatabase")]
public class TransitionDatabaseSO : DatabaseSO<ETransitionType, TransitionBase> { }