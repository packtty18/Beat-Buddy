using UnityEngine;

public class BuddyStat : MonoBehaviour
{
    [SerializeField] private float _maxHealth;
    [SerializeField] private float _currentHealth;
    [SerializeField] private float _damage;

    public void SetStat(BuddyStatDataSO statData)
    {
        _maxHealth = statData.MaxHealth * statData.HealthRate / 100;
        _currentHealth = _maxHealth;
        _damage = statData.Damage * statData.DamageRate / 100;
    }

    public void DecreaseHealth(float value)
    {
        _currentHealth = Mathf.Max(0, _currentHealth - value);
    }

    public bool isDefeated()
    {
        return _currentHealth <= _maxHealth * 0.5f;
    }
    public float GetDamage()
    {
        return _damage;
    }
}
