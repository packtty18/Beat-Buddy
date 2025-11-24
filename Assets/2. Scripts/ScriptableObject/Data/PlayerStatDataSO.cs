using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStatData", menuName = "SO/PlayerStat")]
public class PlayerStatDataSO : ScriptableObject
{
    [Header("Base Stats")]
    public float MaxHealth;                //최대체력
    public float BaseDamage;                //교감시 줄 데미지
    public float AttackGuage;           //교감에 필요한 노트 수
    public float BaseAttackIncrease;        //히트 당 증가하는 기본 attackGuage량
    public float FeverGuage;            //피버에 필요한 노트 수
    public float BaseFeverIncrease;     //히트당 증가하는 기본 피버게이지량
    public float FeverAttackGuagePlus; //피버시 히트당 증가 공격게이지가 증가하는 비율
    public float FeverDamagePlus;     //피버시 증가하는 공격력의 비율
    public float Heal;                  //노트 히트 시 증가하는 힐량 good은 이 수치의 50%, greate는 100%

    public void CopyStat(PlayerStatDataSO statData)
    {
        MaxHealth = statData.MaxHealth;
        BaseDamage = statData.BaseDamage;
        AttackGuage = statData.AttackGuage;
        BaseAttackIncrease = statData.BaseAttackIncrease;
        FeverGuage = statData.FeverGuage;
        BaseFeverIncrease = statData.BaseFeverIncrease;
        FeverAttackGuagePlus = statData.FeverAttackGuagePlus;
        FeverDamagePlus = statData.FeverDamagePlus;
        Heal = statData.Heal;
    }
}
