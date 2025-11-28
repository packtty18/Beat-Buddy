using System.Collections.Generic;
using UnityEngine;

public class UISelector : MonoBehaviour
{
    private const float PADDING_OFFSET = 10;
    [Header("UI Elements")]
    [SerializeField] private List<RectTransform> _targets = new List<RectTransform>();

    [Header("Highlight Prefab")]
    [SerializeField] private GameObject _selectionPrefab;

    [Header("Selector State")]
    [SerializeField] private bool _isActive = false;

    [Header("Debug")]
    [SerializeField] private int _currentIndex = -1;

    private RectTransform _currentTarget;
    private UISelector _prevSelector;
    private RectTransform _highlightUI;

    // 한 프레임 입력 스킵 플래그
    private bool _skipNextFrame = false;

    private void Start()
    {
        Init();
    }

    public void Init()
    {
        _currentIndex = -1;
        InitHighlight();
    }

    private void InitHighlight()
    {
        if (_selectionPrefab == null || _highlightUI != null)
        {
            return;
        }

        GameObject obj = Instantiate(_selectionPrefab, _targets[0]);
        _highlightUI = obj.GetComponent<RectTransform>();
        _highlightUI.gameObject.SetActive(false);
    }
    public void SetActiveSelector()
    {
        _isActive = true;
        gameObject.SetActive(true);
        UpdateHighlight();
    }

    public void DeactivateSelector()
    {
        _isActive = false;
        gameObject.SetActive(false);

        if (_highlightUI != null)
        {
            _highlightUI.gameObject.SetActive(false);
        }
    }

    public void GoToNextSelector(UISelector next)
    {
        if (next == null)
        {
            return;
        }

        _isActive = false;
        
        next.RegisterPrevious(this);
        next.SetActiveSelector();
        next.Init();
    }

    public void ReturnToPrevious()
    {
        if (_prevSelector == null)
        {
            return;
        }

        DeactivateSelector();
        
        _prevSelector.SetActiveSelector();
        _prevSelector._skipNextFrame = true;
    }

    private void RegisterPrevious(UISelector selector)
    {
        _prevSelector = selector;
    }

    private void Update()
    {
        if (_skipNextFrame)
        {
            _skipNextFrame = false;
            return; // 한 프레임 입력 무시
        }

        if (!_isActive || _targets.Count == 0 || !InputManager.IsManagerExist())
        {
            return;
        }

        HandleMoveInput();
        HandleControlInput();
    }

    private ISelectable _lastSelect;

    //상하로 다음 UI로 이동
    private void HandleMoveInput()
    {
        if (InputManager.Instance.GetKeyDown(EGameKeyType.Up))
        {
            if (_lastSelect != null)
            {
                _lastSelect.OnDeselected();
            }

            _currentIndex = (_currentIndex - 1 + _targets.Count) % _targets.Count;
            _currentTarget = _targets[_currentIndex];
            UpdateHighlight();
            if (_currentTarget.TryGetComponent(out ISelectable selectable))
            {
                selectable.OnSelected();
                _lastSelect = selectable;
            }
        }
        else if (InputManager.Instance.GetKeyDown(EGameKeyType.Down))
        {
            if (_lastSelect != null)
            {
                _lastSelect.OnDeselected();
            }

            _currentIndex = (_currentIndex + 1) % _targets.Count;
            _currentTarget = _targets[_currentIndex];
            UpdateHighlight();
            if (_currentTarget.TryGetComponent(out ISelectable selectable))
            {
                selectable.OnSelected();
                _lastSelect = selectable;
            }
        }
    }

    //엔터 혹은 좌우 키로 해당 UI 조절
    private void HandleControlInput()
    {
        if(_currentTarget == null)
        {
            return;
        }

        // 확인
        if (InputManager.Instance.GetKeyDown(EGameKeyType.Confirm) &&
            _currentTarget.TryGetComponent(out IUIConfirmable confirmable))
        {
            confirmable.OnConfirm();
        }

        // 좌우
        if (_currentTarget.TryGetComponent(out IUIValueChangeable valueChange))
        {
            if (InputManager.Instance.GetKeyDown(EGameKeyType.Right))
            {
                valueChange.OnValueIncrease();
            }
                

            if (InputManager.Instance.GetKeyDown(EGameKeyType.Left))
                valueChange.OnValueDecrease();
        }
    }

    
    //하이라이트 조절
    private void UpdateHighlight()
    {
        if (_highlightUI == null)
        {
            return;
        }

        RectTransform targetRect = _currentTarget;

        _highlightUI.gameObject.SetActive(true);
        _highlightUI.SetParent(targetRect);
        Vector3 worldPos = _currentTarget.position;
        _highlightUI.position = worldPos;
        _highlightUI.sizeDelta = new Vector3(targetRect.sizeDelta.x + PADDING_OFFSET, targetRect.sizeDelta.y + PADDING_OFFSET);
    }
}
