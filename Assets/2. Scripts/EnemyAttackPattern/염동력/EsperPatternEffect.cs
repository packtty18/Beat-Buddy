using UnityEngine;
using System.Collections;

public class EsperPatternEffect : MonoBehaviour
{
    public static EsperPatternEffect Instance;

    private void Awake()
    {
        Instance = this;
    }
    public static void MakeEsperEffect()
    {
        if (Instance == null) return;
        Instance.StartCoroutine(EsperEffect());
    }

    private static IEnumerator EsperEffect()
    {
        MakeScreenPurple.MakePurpleScreen();
        yield return null;
    }
}
