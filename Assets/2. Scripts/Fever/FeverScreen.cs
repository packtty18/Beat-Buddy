using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FeverScreen : MonoBehaviour
{
    [Header("피버 스크린 이미지")]
    [SerializeField] private SpriteRenderer _feverScreenSpriteRenderer;
    [SerializeField] private Color _feverScreenColour = new Color(0.1f, 0f, 0.12f, 0.6f);

    [Header("피버 진행상태")]
    private bool _isFever = false;
}
