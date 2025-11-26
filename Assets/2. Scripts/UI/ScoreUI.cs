using System.Collections;
using TMPro;
using UnityEngine;

public class ScoreUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private float _minDuration = 0.2f; // 최소 애니메이션 시간
    [SerializeField] private float _maxDuration = 1f;   // 최대 애니메이션 시간
    [SerializeField] private float _scorePerSecond = 2000f; // 점수 단위에 따른 속도

    private float _currentDisplayScore;
    private Coroutine _countCoroutine;

    private void Start()
    {
        if (!ScoreManager.IsManagerExist()) return;

        ScoreManager.Instance.OnScoreChanged += OnScoreChanged;
        SetScoreInstant(ScoreManager.Instance.Score);
    }

    private void OnDestroy()
    {
        if (!ScoreManager.IsManagerExist()) return;
        ScoreManager.Instance.OnScoreChanged -= OnScoreChanged;
    }

    private void OnScoreChanged(float targetScore)
    {
        if (_countCoroutine != null)
        {
            StopCoroutine(_countCoroutine);
        }

        _countCoroutine = StartCoroutine(CountToTarget(targetScore));
    }

    private IEnumerator CountToTarget(float targetScore)
    {
        float startScore = _currentDisplayScore;
        float delta = targetScore - startScore;

        if (delta <= 0f)
        {
            SetScoreInstant(targetScore);
            yield break;
        }

        float duration = Mathf.Clamp(delta / _scorePerSecond, _minDuration, _maxDuration);
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            _currentDisplayScore = Mathf.Lerp(startScore, targetScore, elapsed / duration);
            _text.text = Mathf.FloorToInt(_currentDisplayScore).ToString("N0");
            yield return null;
        }

        // 정확하게 목표치 세팅
        SetScoreInstant(targetScore);
    }

    private void SetScoreInstant(float score)
    {
        _currentDisplayScore = score;
        _text.text = Mathf.FloorToInt(_currentDisplayScore).ToString("N0");
    }
}
