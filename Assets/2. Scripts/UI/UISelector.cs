using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISelector : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private List<Button> buttons = new List<Button>();
    [SerializeField] private GameObject selectionUIPrefab;
    [SerializeField] private Transform _canvas;

    [SerializeField] private RectTransform selectionUI;
    [SerializeField] private int currentIndex = 0;

    private void Start()
    {
        if (buttons.Count == 0)
        {
            Debug.LogWarning("Buttons or Selection UI not assigned!");
            return;
        }

        UpdateSelectionUI();
    }

    private void Update()
    {
        if (buttons.Count == 0)
        {
            return;
        }

        if(!InputManager.IsManagerExist())
        {
            return;
        }

        if (InputManager.Instance.GetKeyDown(EGameKeyType.Up))
        {
            currentIndex--;
            if (currentIndex < 0) currentIndex = buttons.Count - 1;
            UpdateSelectionUI();
        }

        // ↓ 키 이동
        if (InputManager.Instance.GetKeyDown(EGameKeyType.Down))
        {
            currentIndex++;
            if (currentIndex >= buttons.Count) currentIndex = 0;
            UpdateSelectionUI();
        }

        // Enter 클릭
        if (InputManager.Instance.GetKeyDown(EGameKeyType.Confirm))
        {
            buttons[currentIndex].onClick?.Invoke();
        }
    }

    private void UpdateSelectionUI()
    {
        RectTransform btnRect = buttons[currentIndex].GetComponent<RectTransform>();
        if (selectionUI == null)
        {
            selectionUI = Instantiate(selectionUIPrefab, btnRect).GetComponent<RectTransform>();
        }

        // 선택 UI 위치와 크기를 현재 버튼에 맞춤

        selectionUI.SetParent(btnRect,false);
        selectionUI.localPosition = Vector3.zero;
        selectionUI.sizeDelta = btnRect.sizeDelta;
    }

}
