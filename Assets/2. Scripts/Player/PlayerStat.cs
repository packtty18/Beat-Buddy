using System;
using TMPro.EditorUtilities;
using UnityEngine;

public class PlayerStat : MonoBehaviour
{
    [SerializeField] private float _maxHealth;
    [SerializeField] private float _currentHealth;

    [SerializeField] private float _baseDamage;
    [SerializeField] private float _FeverDamagePlus;

    [SerializeField] private float _maxAttackGuage;
    [SerializeField] private float _currentAttackGuage;
    [SerializeField] private float _baseAttackIncrease;
    [SerializeField] private float _FeverAttackGuagePlus;

    [SerializeField] private float _maxFeverGuage;
    [SerializeField] private float _baseFeverIncrease;
    [SerializeField] private float _currentFeverGuage;

    [SerializeField] private float _currentHeal;

    [SerializeField] public bool IsFeverOn;
    [SerializeField] public bool IsAttakcOn;

    public event Action StartAttack;

    public void SetStat(PlayerStatDataSO stat)
    {
        _maxHealth = stat.MaxHealth;
        _currentHealth = _maxHealth;

        _baseDamage = stat.BaseDamage;
        _FeverDamagePlus = stat.FeverDamagePlus;

        _maxAttackGuage = stat.AttackGuage;
        _currentAttackGuage = 0;
        _baseAttackIncrease = stat.BaseAttackIncrease;
        _FeverAttackGuagePlus = stat.FeverAttackGuagePlus;

        _maxFeverGuage = stat.FeverGuage;
        _baseFeverIncrease = stat.BaseFeverIncrease;
        _currentFeverGuage = 0;

        _currentHeal = stat.Heal;
    }

    //노트 미스마다 체력 감소
    public void DecreaseHealth(float value)
    {
        _currentHealth = Mathf.Max(0, _currentHealth - value);
    }

    //노트 히트마다 체력 증가
    public void OnHeal(EHitType type)
    {
        float value = 0;
        switch (type)
        {
            case EHitType.Good:
                value = _currentHeal * 0.5f;
                break;
            case EHitType.Perfect:
                value = _currentHeal;
                break;
        }

        _currentHealth = Mathf.Min(_maxHealth, _currentHealth + value);
    }

    public float GetDamage()
    {
        //피버라면 데미지 플러스 적용, 아니라면 일반 데미지
        return IsFeverOn ? _baseDamage + _FeverDamagePlus : _baseDamage;
    }


    public void ResetAttackGuage()
    {
        _currentAttackGuage = 0;
        IsAttakcOn = false;
    }

    public void IncreaseAttackGuage()
    {
        float value = IsFeverOn ? _currentAttackGuage + _baseAttackIncrease + _FeverAttackGuagePlus : _currentAttackGuage + _baseAttackIncrease;
        _currentAttackGuage = Mathf.Min(_maxAttackGuage, value);

        if(_currentAttackGuage == _maxAttackGuage)
        {
            IsAttakcOn = true;
            StartAttack?.Invoke();
        }
    }

    //피버 게이지를 리셋
    public void ResetFeverGuage()
    {
        _currentFeverGuage = 0;
        IsFeverOn = false;
    }

    //해당 수치만큼 피버게이지 증가
    public void IncreaseFeverGuage()
    {
        _currentFeverGuage = Mathf.Min(_maxFeverGuage,  _currentFeverGuage + _baseFeverIncrease);
        if(_currentFeverGuage == _maxFeverGuage)
        {
            IsFeverOn = true;
        }
    }
}
