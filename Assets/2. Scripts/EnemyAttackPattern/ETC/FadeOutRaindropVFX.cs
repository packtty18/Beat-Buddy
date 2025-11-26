using UnityEngine;
using UnityEngine.VFX;
using System.Collections;

public class FadeOutRaindropVFX : MonoBehaviour, IPatternEffect
{
    [Header("VFX")]
    [SerializeField] private VisualEffect _raindropVFX;

    [Header("페이드아웃 옵션")]
    private float _stopRainDuration = 3f;
    private float _startSizeMax;
    private float _startIntensity;


    public void StopRainEffect()
    {
        StartCoroutine(FadeOutRainCoroutine(() => Destroy(gameObject)));
    }

    public IEnumerator FadeOutRainCoroutine(System.Action onComplete)
    {
        _startSizeMax = _raindropVFX.GetFloat("SizeMax");
        _startIntensity = _raindropVFX.GetFloat("Intensity");

        float time = 0f;
        while (time < _stopRainDuration) 
        {
            float t = time / _stopRainDuration;
            float newSizeMax = Mathf.Lerp(_startSizeMax, 0f, t);
            float newIntensity = Mathf.Lerp(_startIntensity, 0f, t);
            _raindropVFX.SetFloat("SizeMax", newSizeMax);
            _raindropVFX.SetFloat("Intensity", newIntensity);

            time += Time.deltaTime;
            yield return null;
        }
    }
}

internal interface IPatternEffect
{
    void StopRainEffect();
}

