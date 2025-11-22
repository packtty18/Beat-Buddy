using System;
using System.Collections.Generic;
using UnityEngine;

public enum EGameKeyType
{
    Up,
    Down,
    Left,
    Right,
    Confirm,
    Setting
}

public class InputManager : SimpleSingleton<InputManager>
{
    // 각 게임 키에 대응하는 KeyCode 배열
    private Dictionary<EGameKeyType, KeyCode[]> _keyMapping = new Dictionary<EGameKeyType, KeyCode[]>()
    {
        { EGameKeyType.Up,      new KeyCode[]{ KeyCode.UpArrow, KeyCode.W } },
        { EGameKeyType.Down,    new KeyCode[]{ KeyCode.DownArrow, KeyCode.S } },
        { EGameKeyType.Left,    new KeyCode[]{ KeyCode.LeftArrow, KeyCode.A } },
        { EGameKeyType.Right,   new KeyCode[]{ KeyCode.RightArrow, KeyCode.D } },
        { EGameKeyType.Confirm, new KeyCode[]{ KeyCode.Return, KeyCode.Space } },
        { EGameKeyType.Setting, new KeyCode[]{ KeyCode.Escape } },
    };

    // 키 상태 저장
    private Dictionary<EGameKeyType, bool> _currentDownStates = new Dictionary<EGameKeyType, bool>();
    private Dictionary<EGameKeyType, bool> _previousDownStates = new Dictionary<EGameKeyType, bool>();

    protected override void Awake()
    {
        base.Awake();
        // 모든 키 초기화
        foreach (var key in Enum.GetValues(typeof(EGameKeyType)))
        {
            _currentDownStates[(EGameKeyType)key] = false;
            _previousDownStates[(EGameKeyType)key] = false;
        }
    }

    private void Update()
    {
        // 이전 상태 저장
        foreach (var key in Enum.GetValues(typeof(EGameKeyType)))
        {
            _previousDownStates[(EGameKeyType)key] = _currentDownStates[(EGameKeyType)key];
        }

        // 현재 상태 갱신
        foreach (var kvp in _keyMapping)
        {
            EGameKeyType gameKey = kvp.Key;
            KeyCode[] codes = kvp.Value;

            _currentDownStates[gameKey] = false;

            foreach (var code in codes)
            {
                if (Input.GetKey(code))
                {
                    _currentDownStates[gameKey] = true;
                    break; 
                }
            }
        }
    }

    //단타
    public bool GetKeyDown(EGameKeyType key)
    {
        //키가 현재 눌렸지만 이전 프레임에 눌리지 않았을 경우.
        return _currentDownStates[key] && !_previousDownStates[key];
    }

    //홀드
    public bool GetKey(EGameKeyType key)
    {
        //키가 현재 눌려지는 경우.
        return _currentDownStates[key];
    }

    //릴리즈
    public bool GetKeyUp(EGameKeyType key)
    {
        //키가 현재 눌려지지 않았으나 이전 프레임에 눌려져 있었을 경우
        return !_currentDownStates[key] && _previousDownStates[key];
    }
}
