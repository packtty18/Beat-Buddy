using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class NoteController : MonoBehaviour
{
    [SerializeField] private NoteSpawner _noteSpawner;

    // 랜덤 N개
    public List<Note> GetRandomNotes(int count)
    {
        return _noteSpawner.GetActiveNotes()
            .OrderBy(x => Random.value)
            .Take(count)
            .ToList();
    }

    // 가장 가까운 N개
    public List<Note> GetClosestNotes(int count)
    {
        return _noteSpawner.GetActiveNotes()
            .OrderBy(note => note.GetDistanceToTarget())
            .Take(count)
            .ToList();
    }

    // 특정 타입만
    public List<Note> GetNotesByType(ENoteType type)
    {
        return _noteSpawner.GetActiveNotes()
            .Where(note => note.NoteType == type)
            .ToList();
    }

    // 진행도 범위로
    public List<Note> GetNotesByProgress(float minProgress, float maxProgress)
    {
        return _noteSpawner.GetActiveNotes()
            .Where(note => {
                float progress = note.GetProgressToTarget();
                return progress >= minProgress && progress <= maxProgress;
            })
            .ToList();
    }

    // 다음에 올 N개
    public List<Note> GetUpcomingNotes(int count)
    {
        return _noteSpawner.GetActiveNotes()
            .OrderBy(note => note.TargetBeat)
            .Take(count)
            .ToList();
    }
}
