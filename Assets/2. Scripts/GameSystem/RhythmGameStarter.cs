using UnityEngine;

/// 리듬게임 시작 및 테스트용 스크립트
public class RhythmGameStarter : MonoBehaviour
{

    private void Start()
    {
        // Conductor가 BGMDataSO를 가지고 있으므로 자동으로 BPM 설정됨
        Conductor.Instance.PlayBGM();

        Debug.Log("게임 시작!");
    }

    void Update()
    {
        // 스페이스바: 현재 타이밍 정보
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log($"현재 시간: {Conductor.Instance.BgmPosition:F3}초");
            Debug.Log($"현재 비트: {Conductor.Instance.BgmPositionInBeats:F3}");
        }

        // P키: 일시정지/재개
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (Conductor.Instance.IsPlaying())
            {
                Conductor.Instance.PauseBGM();
            }
            else
            {
                Conductor.Instance.ResumeBGM();
            }
        }
    }
}
