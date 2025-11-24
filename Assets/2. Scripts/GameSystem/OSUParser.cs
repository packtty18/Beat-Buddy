using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class OsuParser
{
    public static OsuBeatmapData ParseOsuFile(string filePath)
    {
        if (!File.Exists(filePath))
        {
            Debug.LogError($"OSU 파일을 찾을 수 없습니다: {filePath}");
            return null;
        }

        var data = new OsuBeatmapData();
        bool hitObjectsSection = false;
        bool timingPointsSection = false;

        foreach (var line in File.ReadLines(filePath))
        {
            if (line.StartsWith("AudioFilename:"))
            {
                data.audioFileName = line.Split(':')[1].Trim();
            }
            else if (line.StartsWith("Title:") && !line.StartsWith("TitleUnicode:"))
            {
                data.title = line.Split(':')[1].Trim();
            }
            else if (line.StartsWith("Artist:") && !line.StartsWith("ArtistUnicode:"))
            {
                data.artist = line.Split(':')[1].Trim();
            }
            else if (line.StartsWith("Version:"))
            {
                data.difficultyName = line.Split(':')[1].Trim();
            }
            else if (line.StartsWith("OverallDifficulty:"))
            {
                float.TryParse(line.Split(':')[1].Trim(), out data.overallDifficulty);
            }
            else if (line.StartsWith("[TimingPoints]"))
            {
                timingPointsSection = true;
                hitObjectsSection = false;
                continue;
            }
            else if (line.StartsWith("[HitObjects]"))
            {
                hitObjectsSection = true;
                timingPointsSection = false;
                continue;
            }
            else if (line.StartsWith("["))
            {
                timingPointsSection = false;
                hitObjectsSection = false;
            }

            if (timingPointsSection && !line.StartsWith("//") && !string.IsNullOrWhiteSpace(line))
            {
                var parts = line.Split(',');
                if (parts.Length >= 2)
                {
                    if (float.TryParse(parts[1], out float beatLength) && beatLength > 0)
                    {
                        if (data.bpm == 0)
                        {
                            data.bpm = 60000f / beatLength;
                        }
                    }
                }
            }

            if (hitObjectsSection && !string.IsNullOrWhiteSpace(line) && !line.StartsWith("//"))
            {
                var parsedNote = ParseHitObject(line, data.bpm);
                if (parsedNote != null)
                {
                    data.notes.Add(parsedNote);
                }
            }
        }

        if (data.bpm == 0)
        {
            Debug.LogWarning("BPM을 찾을 수 없습니다. 기본값 120 사용");
            data.bpm = 120f;
        }

        return data;
    }

    private static NoteData ParseHitObject(string line, float bpm)
    {
        try
        {
            var parts = line.Split(',');
            if (parts.Length < 5) return null;

            float timeMs = float.Parse(parts[2]);
            int type = int.Parse(parts[3]);
            int hitSound = int.Parse(parts[4]);

            // Hit Circle만 처리
            if ((type & 1) == 0) return null;

            // 시간을 beat로 변환 (offset 빼지 않음)
            float secPerBeat = 60f / bpm;
            float beat = (timeMs / 1000f) / secPerBeat;

            // HitSound로 타입 판별 (Taiko 모드 기준)
            bool isKat = ((hitSound & 2) != 0) || ((hitSound & 8) != 0);
            ENoteType noteType = isKat ? ENoteType.RNote : ENoteType.LNote;

            return new NoteData(beat, noteType);
        }
        catch (System.Exception ex)
        {
            Debug.LogWarning($"노트 파싱 실패: {line}\n{ex.Message}");
            return null;
        }
    }
}

public class OsuBeatmapData
{
    public string audioFileName;
    public string title;
    public string artist;
    public string difficultyName;
    public float bpm = 0;
    public float overallDifficulty;

    public List<NoteData> notes = new List<NoteData>();
}
