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

        OnAttack(false);
        OnDefeated(false);
    }

    //플레이어에게 피격 당했을 경우 호출.
    public void OnHit()
    {
        _animator.SetTrigger("OnHit");
    }

    //패턴을 시작할 때 호출.
    public void OnAttack(bool isOn)
    {
        _animator.SetBool("OnAttack", isOn);
    }

    //경계도가 0이 되었을 때 호출.
    public void OnDefeated(bool isOn)
    {
        _animator.SetBool("OnDefeated", isOn);
    }
}
