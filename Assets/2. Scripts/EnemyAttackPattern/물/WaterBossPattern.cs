using UnityEngine;
using System.Collections;

public class WaterBossPattern : MonoBehaviour
{
    [Header("물 공격 트리거")]
    public bool _isWaterAttackActive = false;

    [Header("물 효과 프리팹")]
    [SerializeField] private GameObject _raindropsPrefab;
    private GameObject _raindrops;

    [Header("패턴 지속시간")]
    private float _raindropAnimationTime = 8f;


    public void Attack()
    {
        StartCoroutine(StartWaterAttackCoroutine());
    }

    // 물 공격 시작 코루틴
    private IEnumerator StartWaterAttackCoroutine()
    {
        _isWaterAttackActive = true;

        StartWaterAttack();
        yield return new WaitForSeconds(_raindropAnimationTime);
        FinishWaterAttack();

        _isWaterAttackActive = false;
    }

    // 일정 시간이 되면 효과를 활성화하는 메서드
    private void StartWaterAttack()
    {
        _raindrops = Instantiate(_raindropsPrefab);
    }

    // 시간이 지난 후 효과 파괴
    private void FinishWaterAttack()
    {
        Destroy(_raindrops);
    }
}
