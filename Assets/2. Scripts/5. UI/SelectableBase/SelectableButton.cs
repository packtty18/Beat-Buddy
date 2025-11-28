using UnityEngine;
using UnityEngine.UI;

public class SelectableButton : MonoBehaviour,IUIConfirmable
{
    private Button _button;
    [SerializeField] private ESoundType _confirmSFX = ESoundType.None;
    private void Start()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(OnClickFeedback);
    }
    public void OnConfirm()
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

}
