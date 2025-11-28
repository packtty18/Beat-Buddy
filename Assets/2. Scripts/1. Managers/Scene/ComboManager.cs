using DG.Tweening;
using TMPro;
using UnityEngine;

public class ComboManager : SceneSingleton<ComboManager>
{
    [SerializeField] private TextMeshProUGUI _comboInfoText;
    [SerializeField] private TextMeshProUGUI _comboText;
    private int _comboCount;
    public int ComboCount => _comboCount;
    private Sequence _comboSequence;
    private void Start()
    {
        _comboText.alpha = 0f;
        _comboInfoText.alpha = 0f;
        _comboCount = 0;
    }
    public void IncreaseCombo()
    {
        if (_comboSequence != null)
        {
            _comboSequence.Kill();
        }
        _comboCount++;
        _comboText.text = _comboCount.ToString();

        _comboSequence = DOTween.Sequence();
        _comboSequence.Append(_comboText.DOFade(1f, 0.5f));
        _comboSequence.Join(_comboInfoText.DOFade(1f, 0.5f));
        _comboSequence.Join(_comboText.transform.DOScale(1f, 0.5f).From(0.5f).SetEase(Ease.InOutBack));
        _comboSequence.Join(_comboInfoText.transform.DOScale(1f, 0.5f).From(0.5f).SetEase(Ease.InOutBack));
    }
    public void ResetCombo()
    {
        _comboCount = 0;
        _comboText.DOFade(0f, 0.5f);
        _comboInfoText.DOFade(0f, 0.5f);
    }

}
