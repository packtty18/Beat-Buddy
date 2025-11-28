using System.Collections.Generic;
using UnityEngine;

public class BackgroundManager : SceneSingleton<BackgroundManager>
{
    [SerializeField] private List<Sprite> _backgroundSprites;
    [SerializeField] private List<Sprite> _groundSprites;

    [SerializeField] private SpriteRenderer _background = null;
    [SerializeField] private SpriteRenderer _playerGround = null;
    [SerializeField] private SpriteRenderer _buddyGround = null;

    public void SetBackground(ESongType songType)
    {
        switch (songType)
        {
            case ESongType.FlameTheme:
                _background.sprite = _backgroundSprites[0];
                _playerGround.sprite = _groundSprites[0];
                _buddyGround.sprite = _groundSprites[0];
                break;
            case ESongType.WaterTheme:
                _background.sprite = _backgroundSprites[1];
                _playerGround.sprite = _groundSprites[1];
                _buddyGround.sprite = _groundSprites[1];
                break;
            case ESongType.ThunderTheme:
                _background.sprite = _backgroundSprites[2];
                _playerGround.sprite = _groundSprites[2];
                _buddyGround.sprite = _groundSprites[2];
                break;
            case ESongType.EsperTheme:
                _background.sprite = _backgroundSprites[3];
                _playerGround.sprite = _groundSprites[3];
                _buddyGround.sprite = _groundSprites[3];
                break;
            case ESongType.FinalTheme:
                _background.sprite = _backgroundSprites[4];
                _playerGround.sprite = _groundSprites[4];
                _buddyGround.sprite = _groundSprites[4];
                break;
            default:
                _background.sprite = _backgroundSprites[5];
                _playerGround.sprite = _groundSprites[5]; 
                _buddyGround.sprite = _groundSprites[5]; 
                break;
        }
    }
}
