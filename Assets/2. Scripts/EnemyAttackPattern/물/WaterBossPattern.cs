using UnityEngine;
using System.Collections;

public class WaterBossPattern : MonoBehaviour
{
    [Header("물 공격 트리거")]
    [SerializeField] private bool _isWaterAttackActive = false;

    [Header("물 효과 프리팹")]
    [SerializeField] private GameObject _raindropsPrefab;
    private GameObject _raindrops;

    [Header("패턴 지속시간")]
    private float _raindropAnimationTime = 8f;


    public void Attack()
    {
        StartCoroutine(StartWaterAttackCoroutine());
        Debug.Log("어택");
    }

    private IEnumerator StartWaterAttackCoroutine()
    {
        _isWaterAttackActive = true;

        _raindrops = Instantiate(_raindropsPrefab);
        BuddyManager.Instance.StartBuddyPattern(true);
        yield return new WaitForSeconds(_raindropAnimationTime);
        StopRain();
        BuddyManager.Instance.StartBuddyPattern(false);
        _isWaterAttackActive = false;
    }

    // 스킬 멈출 때 비 천천히 사라지는 효과 메서드
    private void StopRain()
    {
        var effect = _raindrops.GetComponent<IPatternEffect>();
        if (effect != null)
        {
            effect.StopRainEffect();
        }
    }
}
