using System.Collections.Generic;
using UnityEngine;

public class UpgradeUI : MonoBehaviour
{
    [SerializeField] private List<UpgradeOptionButtonUI> _optionButtons;
    [SerializeField] private GameObject _rootPanel;

    private List<UpgradeOptionSO> _currentOptions;

    [ContextMenu("SetChoice")]
    public void ShowUpgradeChoices()
    {
        _rootPanel.SetActive(true);

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

        Close();
    }

    public void Close()
    {
        _rootPanel.SetActive(false);
    }
}
