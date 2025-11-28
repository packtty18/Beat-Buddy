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

    // 진행도 범위 내 랜덤하게
    public List<Note> GetRandomNotesByProgress(float minProgress, float maxProgress, int count)
    {
        var notesInRange = _noteSpawner.GetActiveNotes()
            .Where(note => {
                float progress = note.GetProgressToTarget();
                return progress >= minProgress && progress <= maxProgress;
            })
            .ToList();

        // 범위 내 노트가 없으면 빈 리스트 반환
        if (notesInRange.Count == 0)
        {
            Debug.LogWarning($"[NoteController] 진행도 {minProgress:F2}~{maxProgress:F2} 범위에 노트가 없습니다!");
            return new List<Note>();
        }

        // 요청한 개수보다 적으면 있는 만큼만 반환
        int actualCount = Mathf.Min(count, notesInRange.Count);

        if (actualCount < count)
        {
            Debug.LogWarning($"[NoteController] 요청: {count}개, 실제: {actualCount}개 (범위 내 노트 부족)");
        }

        return notesInRange
            .OrderBy(x => Random.value)
            .Take(actualCount)
            .ToList();
    }
}
