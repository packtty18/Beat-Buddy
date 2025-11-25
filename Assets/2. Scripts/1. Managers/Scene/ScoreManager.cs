using System;
using UnityEngine;

public class ScoreManager : SceneSingleton<ScoreManager>
{
    [SerializeField] private float _score;
    [SerializeField] private FloatingScoreText _floatingScorePrefab;
    [SerializeField] private Transform _floatingScoreParent;

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
        if (_floatingScorePrefab == null || _floatingScoreParent == null) 
            return;

        FloatingScoreText floating = Instantiate(_floatingScorePrefab, _floatingScoreParent);
        floating.Initialize("+" + increase.ToString("N0"));
    }

    public string GetScoreInString() => _score.ToString("N0");
}
