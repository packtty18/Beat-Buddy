using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeOptionButtonUI : SelectableButton
{
    [SerializeField] private Image _iconImage;
    [SerializeField] private TextMeshProUGUI _titleText;
    [SerializeField] private TextMeshProUGUI _descriptionText;
    [SerializeField] private Button _button;

    private UpgradeOptionSO _optionData;
    private Action<UpgradeOptionSO> _onConfirmUpgrade;

    public override void OnConfirm()
    {
        Debug.Log($"[OptionButtonUI] Player clicked option : {_optionData.name}");
        _onConfirmUpgrade?.Invoke(_optionData);
    }

    public void SetOption(UpgradeOptionSO optionData, Action<UpgradeOptionSO> onSelected)
    {
        _optionData = optionData;
        _onConfirmUpgrade = onSelected;
        _iconImage.sprite = optionData.Icon;
        _titleText.text = optionData.Name;
        _descriptionText.text = optionData.GetDescription();

        Debug.Log($"[OptionButtonUI] Assigned Option : {_optionData.name}");

        _button.onClick.RemoveAllListeners();
        _button.onClick.AddListener(OnConfirm);
    }

}
