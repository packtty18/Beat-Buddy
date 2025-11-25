using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UpgradeChoiceGroup
{
    [SerializeField] private List<UpgradeOptionSO> Options;

    public List<UpgradeOptionSO> OptionsList => Options;
}

[CreateAssetMenu(fileName = "UpgradeDatabase", menuName = "SO/UpgradeDatabase")]
public class UpgradeDatabaseSO : ScriptableObject
{
    [SerializeField] private List<UpgradeChoiceGroup> _upgradeChoice;

    public List<UpgradeChoiceGroup> GetUpgradeData()
    {
        return _upgradeChoice;
    }

}
