using UnityEngine;

public enum ETransitionType
{
    Fade,
}

[CreateAssetMenu(fileName = "TransitionDatabase", menuName = "SO/TransitionDatabase")]
public class TransitionDatabaseSO : DatabaseSO<ETransitionType, TransitionBase> { }