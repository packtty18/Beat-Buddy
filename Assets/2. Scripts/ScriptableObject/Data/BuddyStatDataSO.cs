using UnityEngine;

[CreateAssetMenu(fileName = "BuddyStatData", menuName = "SO/BuddyStat")]
public class BuddyStatDataSO : ScriptableObject
{
    [Header("Base Stats")]
    public float MaxHealth;     //최대체력
    public float Damage;        //미스 시 줄 데미지
    public float HealthRate = 100;
    public float DamageRate = 100;

    public void CopyStat(BuddyStatDataSO statData)
    {
        MaxHealth = statData.MaxHealth;
        Damage = statData.Damage;
        HealthRate = statData.HealthRate;
        DamageRate = statData.DamageRate;
    }
}
