using NUnit.Framework.Interfaces;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum EGameKeyType
{
    Up,
    Down,
    Left,
    Right,
    Confirm,
    Setting
}

//역할 : 특정 입력에 대한 여부 확인
public class InputManager : CoreSingleton<InputManager>
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
    private EGameKeyType[] _gameKeyTypes;
    //private bool _isActive = true;

    


    protected override void Awake()
    {
        base.Awake();
        _gameKeyTypes = (EGameKeyType[])Enum.GetValues(typeof(EGameKeyType));

        // 모든 키 초기화
        foreach (var key in Enum.GetValues(typeof(EGameKeyType)))
        {
            _currentDownStates[(EGameKeyType)key] = false;
            _previousDownStates[(EGameKeyType)key] = false;
        }
    }

    private void Update()
    {
        //인풋 비활성화시 로직 중단
        //if (!_isActive)
        //{
        //    return;
        //}

        // 이전 상태 저장
        foreach (EGameKeyType key in _gameKeyTypes)
        {
            _previousDownStates[key] = _currentDownStates[key];
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

    public void SetInputActive(bool tf)
    {
        //_isActive = tf;
    }

    public bool GetAnyKey()
    {
        //키가 현재 눌렸지만 이전 프레임에 눌리지 않았을 경우.
        //눌렀을때 한프레임만 true를 반환
        return GetKeyDown(EGameKeyType.Left) || GetKeyDown(EGameKeyType.Right) ||
            GetKeyDown(EGameKeyType.Down) || GetKeyDown(EGameKeyType.Up) ||
            GetKeyDown(EGameKeyType.Confirm) || GetKeyDown(EGameKeyType.Setting);
    }

    public bool GetKeyDown(EGameKeyType key)
    {
        //키가 현재 눌렸지만 이전 프레임에 눌리지 않았을 경우.
        //눌렀을때 한프레임만 true를 반환
        return _currentDownStates[key] && !_previousDownStates[key];
    }

    //홀드
    public bool GetKey(EGameKeyType key)
    {
        //키가 현재 눌려지는 경우.
        //누르고 있는 동안 계속 true를 반환
        return _currentDownStates[key];
    }

    //릴리즈
    public bool GetKeyUp(EGameKeyType key)
    {
        //키가 현재 눌려지지 않았으나 이전 프레임에 눌려져 있었을 경우
        //뗐을때 한프레임만 true를 반환
        return !_currentDownStates[key] && _previousDownStates[key];
    }
}
