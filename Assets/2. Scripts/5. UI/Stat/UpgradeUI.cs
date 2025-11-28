using System;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeUI : MonoBehaviour
{
    [SerializeField] private List<UpgradeOptionButtonUI> _optionButtons;

    private List<UpgradeOptionSO> _currentOptions;

    // 선택 완료 여부
    public bool IsSelected { get; private set; }

    // 선택된 옵션 전달용 이벤트
    public event Action<UpgradeOptionSO> OnSelected;

    public void ShowUpgradeChoices()
    {
        IsSelected = false;       // 선택 초기화
        gameObject.SetActive(true);

        _currentOptions = UpgradeManager.Instance.GetRandomUpgradeOption();

        if (_currentOptions == null || _currentOptions.Count == 0)
        {
            Debug.LogWarning("[UpgradeUI] No upgrade options found!");
            return;
        }

        for (int i = 0; i < _optionButtons.Count; i++)
        {
            if (i < _currentOptions.Count)
            {
                _optionButtons[i].gameObject.SetActive(true);
                _optionButtons[i].SetOption(_currentOptions[i], OnOptionSelected);
            }
            else
            {
                _optionButtons[i].gameObject.SetActive(false);
            }
        }
    }

    private void OnOptionSelected(UpgradeOptionSO selectedOption)
    {
        Debug.Log($"[UpgradeUI] Option Selected : {selectedOption.name}");

        UpgradeManager.Instance.AddUpgradeOption(selectedOption);

        IsSelected = true;    // 선택 완료
        OnSelected?.Invoke(selectedOption);

        Close();
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }
}
