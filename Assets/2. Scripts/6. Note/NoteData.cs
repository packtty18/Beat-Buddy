using System;
using UnityEngine;
public enum ENoteType
{
    LNote,  // 왼쪽 노트
    RNote,   // 오른쪽 노트
    
}

[Serializable]
public class NoteData
{
    [Tooltip("노트가 판정선에 도달할 비트")]
    public float beat;

    [Tooltip("노트 타입 (좌/우)")]
    public ENoteType type;

    public NoteData(float beat, ENoteType type)
    {
        this.beat = beat;
        this.type = type;
    }
}
