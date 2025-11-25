using System;
using UnityEngine;

public class ScoreManager : SceneSingleton<ScoreManager>
{
    [SerializeField] private float _score;
    [SerializeField] private FloatingScoreTextUI _floatingScorePrefab;
    [SerializeField] private Transform _floatingParent;

    public float Score => _score;
    public event Action<float> OnScoreChanged;

    public void IncreaseScore(float increase)
    {
        SetScore(_score + increase);
        ShowFloatingScore(increase);
    }

    public void ResetScore()
    {
        SetScore(0);
    }

    private void SetScore(float value)
    {
        _score = Mathf.Max(0, value);
        OnScoreChanged?.Invoke(_score);
    }

    private void ShowFloatingScore(float increase)
    {
        if (!PoolManager.IsManagerExist())
        {
            return;
        }

        FloatingScoreTextUI floating = PoolManager.Instance.SpawnGetComponent<UIPool, EUIType, FloatingScoreTextUI>(EUIType.FloatingScoreText);
        floating.Initialize("+" + increase.ToString("N0"), _floatingParent);
        
    }

    public string GetScoreInString() => _score.ToString("N0");
}
