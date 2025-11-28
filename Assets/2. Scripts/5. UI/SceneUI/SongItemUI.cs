using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SongItemUI : SelectableButton
{
    [Header("UI References")]
    [SerializeField] private Image _iconImage;           // 곡 대표 이미지 (추후 추가)
    [SerializeField] private TextMeshProUGUI _titleText; // 곡 제목
    [SerializeField] private Transform _difficultyRoot;  // 난이도 표시 부모
    [SerializeField] private GameObject _difficultyIconPrefab; // 난이도 아이콘 프리팹

    private BGMDataSO _data;

    /// <summary>
    /// SongItemUI 초기화
    /// </summary>
    public void SetData(BGMDataSO data)
    {
        _data = data;

        _iconImage.sprite = data.BgmIcon;
        _titleText.text = "Title : " + _data.BgmName;

        for (int i = 0; i < _data.Difficulty; i++)
        {
            GameObject icon = Instantiate(_difficultyIconPrefab, _difficultyRoot);
            icon.transform.localScale = Vector3.one;
        }
    }

    /// <summary>
    /// 곡 선택 시 호출
    /// </summary>
    public override void OnConfirm()
    {
        if (_data == null)
        {
            Debug.LogWarning("SongItemUI: BGMDataSO가 할당되지 않았습니다.");
            return;
        }

        SongManager.Instance.SelectSongByIndex((int)_data.SongType -1 );
        GameManager.Instance.StartStage(); // StartStage() -> StartGame()
    }

    public AudioClip GetAudioClip()
    {
        return _data.AudioClip;
    }
}
