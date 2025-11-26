using UnityEngine;
using UnityEngine.UI;

public class SelectableButton : MonoBehaviour,IUIConfirmable
{
    private Button _button;
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
    }

}
