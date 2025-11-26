using UnityEngine;
using System.Collections;

public class EsperPatternEffect : MonoBehaviour
{
    public static EsperPatternEffect Instance;

    [Header("염동력 프리팹")]
    [SerializeField] private GameObject _esperWavePrefab;
    private GameObject _currentEsperEffect;

    [Header("이펙트 지속시간")]
    private float _esperEffectTime = 8f;

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
        Instance.EsperWaveEffectOn();
        yield return null;
    }

    private void EsperWaveEffectOn()
    {
        _currentEsperEffect = Instantiate(_esperWavePrefab);
        Destroy(_currentEsperEffect, _esperEffectTime);
    }
}
