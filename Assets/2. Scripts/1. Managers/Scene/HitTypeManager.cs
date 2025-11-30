using System;
using UnityEngine;

public class HitTypeManager : SceneSingleton<HitTypeManager>
{
    [SerializeField] private string _text;
    [SerializeField] private Transform _floatingParent;
    private int _hitTypeIndex;

    public void SetText(EHitType type)
    {
        switch (type)
        {
            case EHitType.Perfect:
                _text = "Perfect";
                _hitTypeIndex = 0;
                break;
            case EHitType.Good:
                _text = "Good";
                _hitTypeIndex = 1;
                break;
            case EHitType.Bad:
                _text = "Bad";
                _hitTypeIndex = 2;
                break;
            case EHitType.Miss:
                _text = "Miss";
                _hitTypeIndex = 3;
                break;
            default:
                break;
        }
        ShowFloatingScore();
    }

    private void ShowFloatingScore()
    {
        if (!PoolManager.IsManagerExist())
        {
            return;
        }

        FloatingHitTypeTextUI floating = PoolManager.Instance.SpawnGetComponent<HitTypePool, EHitEffectText, FloatingHitTypeTextUI>(EHitEffectText.FloatingHitTypeText);
        floating.Initialize(_text, _floatingParent, _hitTypeIndex);
    }
}
