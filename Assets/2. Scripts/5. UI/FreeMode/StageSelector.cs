using UnityEngine;
using System.Collections.Generic;

public class StageSelector : MonoBehaviour , IUIConfirmable, IUIValueChangeable
{
    [Header("Song Items")]
    [SerializeField] private GameObject _itemPrefab;
    [SerializeField] private Transform _instantRoot;
    [SerializeField] private BGMDataSO[] _songDatas;
    [SerializeField] private List<RectTransform> songItems;

    [Header("Circular Settings")]
    [SerializeField] private float radius = 250f;
    [SerializeField] private float angleStep = 25f;
    [SerializeField] private Vector2 itemOffset = new Vector2(0, 30f);

    [Header("Center Position")]
    [SerializeField] private Vector2 centerPos = new Vector2(-1f, 0f); // 중앙 위치 조절 가능

    [Header("Animation")]
    [SerializeField] private float moveLerpSpeed = 8f;
    [SerializeField] private float scaleLerpSpeed = 10f;

    [Header("Visible Settings")]
    [SerializeField] private int sideVisibleCount = 2;

    private int currentIndex = 0;
    private int itemCount;

    private void Start()
    {
        _songDatas = SongManager.Instance.GetAllSongs();
        currentIndex = 0;
        
        InstantSongItem();
        itemCount = songItems.Count;
        UpdateItemPositions(true); // 초기 위치 강제 적용
    }

    private void InstantSongItem()
    {
        foreach (BGMDataSO data in _songDatas)
        {
            GameObject item = Instantiate(_itemPrefab, _instantRoot);
            item.GetComponent<SongItemUI>().SetData(data);
            songItems.Add(item.GetComponent<RectTransform>());
        }
    }

    private void Update()
    {
        UpdateItemPositions(false);
    }


    private int LoopIndex(int value)
    {
        if (value < 0) 
            value = itemCount - 1;
        else if (value >= itemCount) 
            value = 0;
        return value;
    }

    private void UpdateItemPositions(bool instant)
    {
        for (int i = 0; i < itemCount; i++)
        {
            int rawOffset = i - currentIndex;

            if (rawOffset < -itemCount / 2) rawOffset += itemCount;
            if (rawOffset > itemCount / 2) rawOffset -= itemCount;

            bool shouldVisible = Mathf.Abs(rawOffset) <= sideVisibleCount;
            RectTransform item = songItems[i];

            float angle = Mathf.Deg2Rad * (rawOffset * angleStep); // 반시계 방향
            Vector2 circlePos = new Vector2(
                Mathf.Sin(angle) * radius, // 왼쪽 원형
                Mathf.Cos(angle) * radius
            );

            Vector2 finalPos = centerPos + circlePos + (itemOffset * rawOffset);

            // 비활성화 상태에서 위치 강제 세팅 후 활성화
            if (shouldVisible)
            {
                if (!item.gameObject.activeSelf)
                {
                    item.anchoredPosition = finalPos;
                    item.localScale = Vector3.one * ((rawOffset == 0) ? 1.2f : 0.9f);
                    item.gameObject.SetActive(true);
                }
            }
            else
            {
                if (item.gameObject.activeSelf)
                    item.gameObject.SetActive(false);
                continue;
            }

            // Lerp 적용
            if (!instant)
            {
                item.anchoredPosition = Vector2.Lerp(
                    item.anchoredPosition,
                    finalPos,
                    Time.deltaTime * moveLerpSpeed
                );

                float targetScale = (rawOffset == 0) ? 1.2f : 0.9f;
                item.localScale = Vector3.Lerp(
                    item.localScale,
                    Vector3.one * targetScale,
                    Time.deltaTime * scaleLerpSpeed
                );
            }
            else
            {
                item.anchoredPosition = finalPos;
            }

            // 수정
            int siblingIndex = sideVisibleCount - Mathf.Abs(rawOffset) + (rawOffset == 0 ? sideVisibleCount : 0);
            siblingIndex = Mathf.Clamp(siblingIndex, 0, sideVisibleCount * 2);
            item.SetSiblingIndex(siblingIndex);
        }
    }
    public void OnConfirm()
    {
        songItems[currentIndex].GetComponent<SongItemUI>().OnConfirm();
    }

    public void OnValueIncrease()
    {
        currentIndex = LoopIndex(currentIndex + 1);
        SoundManager.Instance.PlayBGM(GetCurrentClip());
    }

    public AudioClip GetCurrentClip()
    {
        return songItems[currentIndex].GetComponent<SongItemUI>().GetAudioClip();
    }

    public void OnValueDecrease()
    {
        currentIndex = LoopIndex(currentIndex - 1);
        SoundManager.Instance.PlayBGM(GetCurrentClip());
    }
}
