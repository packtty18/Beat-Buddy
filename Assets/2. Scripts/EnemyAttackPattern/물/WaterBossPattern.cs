using UnityEngine;
using System.Collections;

public class WaterBossPattern : MonoBehaviour
{
    [Header("물 공격 트리거")]
    [SerializeField] private bool _isWaterAttackActive = false;

    [Header("물 효과 프리팹")]
    [SerializeField] private GameObject _raindropsPrefab;
    [SerializeField] private GameObject _raindropsStagePrefab;
    private GameObject _raindrops;
    private GameObject _raindropsStage;

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
        SoundManager.Instance.PlaySFX(ESoundType.SFX_WaterStart);
        _raindrops = Instantiate(_raindropsPrefab);
        _raindropsStage = Instantiate(_raindropsStagePrefab);
        BuddyManager.Instance.StartBuddyAttackAnimation(true);
        SoundManager.Instance.PlaySFX(ESoundType.SFX_WaterRainDrop);
        yield return new WaitForSeconds(_raindropAnimationTime);
        StopRain(_raindrops);
        StopRain(_raindropsStage);
        BuddyManager.Instance.StartBuddyAttackAnimation(false);

        _isWaterAttackActive = false;
    }

    // 스킬 멈출 때 비 천천히 사라지는 효과 메서드
    private void StopRain(GameObject rain)
    {
        if (rain != null)
        {
            var effect = rain.GetComponent<IPatternEffect>();
            if (effect != null)
            {
                effect.StopRainEffect();
            }
        }
    }
}
