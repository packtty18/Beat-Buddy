using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SelectableButton : MonoBehaviour,IUIConfirmable
{
    private Button _button;
    [SerializeField] private ESoundType _confirmSFX = ESoundType.None;
    [SerializeField] private UnityEvent selectEvent;
    [SerializeField] private UnityEvent _deselectEvent;
    private void Start()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(OnClickFeedback);
    }
    public virtual void OnConfirm()
    {
        _button.onClick.Invoke();
    }

    protected virtual void OnClickFeedback()
    {
        Debug.Log($"{gameObject.name} : ButtonClicked");

        if(_confirmSFX != ESoundType.None)
        {
            SoundManager.Instance.PlaySFX( _confirmSFX );
        }
    }

    public virtual void OnSelected()
    {
        Debug.Log($"{gameObject.name} : OnSelected");
        SoundManager.Instance.PlaySFX(ESoundType.SFX_ButtonSelect);
        selectEvent.Invoke();
    }

    public virtual void OnDeselected()
    {
        Debug.Log($"{gameObject.name} : OnDeselected");
        _deselectEvent.Invoke();
    }
}
