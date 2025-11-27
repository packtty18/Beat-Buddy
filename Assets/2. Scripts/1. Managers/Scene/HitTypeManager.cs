using System;
using UnityEngine;

public class HitTypeManager : SceneSingleton<HitTypeManager>
{
    [SerializeField] private string _text;
    [SerializeField] private FloatingHitTypeTextUI _floatinghHitTypePrefab;
    [SerializeField] private Transform _floatingParent;
    public string Text => _text;

    public void SetText(EHitType type)
    {
        switch (type)
        {
            case EHitType.Perfect:
                _text = "Perfect";
                break;
            case EHitType.Good:
                _text = "Good";
                break;
            case EHitType.Bad:
                _text = "Bad";
                break;
            case EHitType.Miss:
                _text = "Miss";
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
        floating.Initialize(_text, _floatingParent);
    }
}
