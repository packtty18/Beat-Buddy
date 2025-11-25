using System;
using System.Collections.Generic;
using UnityEngine;

public class StatManager : SceneSingleton<StatManager>
{
    [Header("Default Stats")]
    [SerializeField] private PlayerStatDataSO _defaultPlayerStat;             //플레이어 기본 스텟
    [SerializeField] private BuddyStatDataSO[] _stageDefaultBuddyStats;          //버디 기본 스텟

    [Header("Runtime Stats")]
    [SerializeField] private PlayerStat _playerStat;                    //현재 스테이지의 플레이어 스텟
    [SerializeField] private BuddyStat _buddyStat;                      //현재 스테이지의 버디 스텟

    [Header("Debug")]
    [SerializeField] private int currentStage;


    private List<UpgradeOptionSO> _upgradeOptions => UpgradeManager.Instance.GetUpgradeOption();     //업그레이드 요소

    protected override void Awake()
    {
        base.Awake();  
    }

    /// <summary>
    /// 스테이지별 스탯 초기화 후 업그레이드 적용
    /// </summary>
    [ContextMenu("SetStat")]
    public void TestSetStat()
    {
        SetStat(currentStage);
    }

    public void SetStat(int stage)
    {
        currentStage = stage;
        PlayerStatDataSO _upgradedPlayerStat = ScriptableObject.CreateInstance<PlayerStatDataSO>();
        BuddyStatDataSO _upgradedBuddyStat = ScriptableObject.CreateInstance<BuddyStatDataSO>();

        _upgradedPlayerStat.CopyStat(_defaultPlayerStat);
        _upgradedBuddyStat.CopyStat(_stageDefaultBuddyStats[stage - 1]);

        foreach (var upgrade in _upgradeOptions)
        {
            upgrade.Apply(_upgradedPlayerStat, _upgradedBuddyStat);
        }

        _playerStat.SetStat(_upgradedPlayerStat);
        _buddyStat.SetStat(_upgradedBuddyStat);

        Debug.Log($"[StatManager] Stage {stage} stats applied.");
    }

    public void AddUpgradeOption(UpgradeOptionSO upgrade)
    {
        if (!_upgradeOptions.Contains(upgrade))
            _upgradeOptions.Add(upgrade);
    }
}
