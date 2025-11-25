using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : CoreSingleton<UpgradeManager>
{
    [SerializeField] private UpgradeDatabaseSO _upgradeDatabase;

    [Header("Upgrade Options")]
    [SerializeField] private List<UpgradeOptionSO> _upgradeOptions;     //업그레이드 요소



    [Header("Debug")]
    private int stage = 1;

    protected override void Awake()
    {
        base.Awake();
        if (_upgradeOptions == null)
        {
            _upgradeOptions = new List<UpgradeOptionSO>();
        }
    }

    public List<UpgradeOptionSO> GetRandomUpgradeOption()
    {
        List<UpgradeChoiceGroup> choiceGroups = _upgradeDatabase.GetUpgradeData();

        if (choiceGroups == null || choiceGroups.Count == 0)
        {
            Debug.LogWarning("UpgradeDatabaseSO에 업그레이드 데이터가 없습니다.");
            return new List<UpgradeOptionSO>();
        }

        int index = Mathf.Clamp(stage - 1, 0, choiceGroups.Count - 1);

        var options = choiceGroups[index].OptionsList;
        if (options == null || options.Count == 0)
        {
            Debug.LogWarning($"Stage {stage}에 업그레이드 옵션이 없습니다.");
            return new List<UpgradeOptionSO>();
        }

        // 3개보다 적으면 가능한 개수만 반환
        int count = Mathf.Min(3, options.Count);

        return Util.GetRandomElementsInList(options, count);
    }

    public void ResetUpgradeOption()
    {
        _upgradeOptions.Clear();
    }
    public void AddUpgradeOption(UpgradeOptionSO option)
    {
        _upgradeOptions.Add(option);
    }

    public List<UpgradeOptionSO> GetUpgradeOption()
    {
        return _upgradeOptions;
    }    

} 
