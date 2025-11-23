using UnityEngine;

public class BuddyAnimatorController : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    private void Start()
    {
        Init();
    }

    public void Init()
    {
        if(_animator == null)
        {
            _animator = GetComponent<Animator>();
        }

        OnAttackAnimator(false);
        OnDefeatedAnimator(false);
    }

    //플레이어에게 피격 당했을 경우 호출.
    public void OnHitAnimator()
    {
        _animator.SetTrigger("OnHit");
    }

    //패턴을 시작할 때 호출.
    public void OnAttackAnimator(bool isOn)
    {
        _animator.SetBool("OnAttack", isOn);
    }

    //경계도가 0이 되었을 때 호출.
    public void OnDefeatedAnimator(bool isOn)
    {
        _animator.SetBool("OnDefeated", isOn);
    }
}
