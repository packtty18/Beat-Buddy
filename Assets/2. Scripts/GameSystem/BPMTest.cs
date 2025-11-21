using UnityEngine;

public class BPMTest : MonoBehaviour
{
    public AudioClip clip;

    private void Start()
    {
        int bpm = UniBpmAnalyzer.AnalyzeBpm(clip);
    }
}