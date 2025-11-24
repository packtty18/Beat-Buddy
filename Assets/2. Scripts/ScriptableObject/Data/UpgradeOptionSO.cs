using UnityEngine;

public enum EUpgradeType
{
    HealthUp,
    DamageUp,
    AttackGuageDown,
    FeverGuageDown,
    BuddyDamageDown,
    BuddyHealthDown,
    FeverAttackGuagePlusUp,
    FeverDamagePlusUp,
    HealUp,
}

[CreateAssetMenu(fileName = "UpgradeOption", menuName = "SO/UpgradeOption")]
public class UpgradeOptionSO : ScriptableObject
{
    public EUpgradeType OptionType;
    public float Value;

    public void Apply(PlayerStatDataSO playerStat, BuddyStatDataSO buddyStat)
    {
        Debug.Log($"[Upgrade] Apply {OptionType} : {Value}");

        switch (OptionType)
        {
            case EUpgradeType.HealthUp:
                playerStat.MaxHealth += Value;
                break;
            case EUpgradeType.DamageUp:
                playerStat.BaseDamage += Value;
                break;
            case EUpgradeType.AttackGuageDown:
                playerStat.AttackGuage -= Value;
                break;
            case EUpgradeType.FeverGuageDown:
                playerStat.FeverGuage -= Value;
                break;
            case EUpgradeType.BuddyDamageDown:
                buddyStat.DamageRate -= Value;
                break;
            case EUpgradeType.BuddyHealthDown:
                buddyStat.HealthRate -= Value;
                break;
            case EUpgradeType.FeverAttackGuagePlusUp:
                playerStat.FeverAttackGuagePlus += Value;
                break;
            case EUpgradeType.FeverDamagePlusUp:
                playerStat.FeverDamagePlus += Value;
                break;
            case EUpgradeType.HealUp:
                playerStat.Heal += Value;
                break;
        }
    }
}
