using UnityEngine;

public class PlayerAnimatorController : MonoBehaviour
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

        SetFever(false);
        SetVictory(false);
        SetFail(false);
    }

    //노트를 놓쳐서 피해를 입을 경우
    public void OnHit()
    {
        _animator.SetTrigger("OnHit");
    }

    //교감을 실시할 경우
    public void OnAttack()
    {
        _animator.SetTrigger("OnAttack");
    }

    //피버 모드에 돌입할 경우 혹은 종료할 경우
    public void SetFever(bool isOn)
    {
        _animator.SetBool("OnFever", isOn);
    }

    //스테이지를 승리했을 경우
    public void SetVictory(bool isOn)
    {
        _animator.SetBool("OnVictory", isOn);
    }

    //스테이지를 실패했을 경우
    public void SetFail(bool isOn)
    {
        _animator.SetBool("OnFail", isOn);
    }
}
