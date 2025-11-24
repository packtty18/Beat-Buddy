using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UpgradeChoiceGroup
{
    public UpgradeOptionSO[] Options;
}

[CreateAssetMenu(fileName = "UpgradeDatabase", menuName = "SO/UpgradeDatabase")]
public class UpgradeDatabaseSO : ScriptableObject
{
    [SerializeField] private List<UpgradeChoiceGroup> UpgradeChoice;

}
