using UnityEngine;
using TMPro;

public class FloatingHitTypeTextUI : MonoBehaviour, IPoolable
{
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private float _riseSpeed = 1;
    [SerializeField] private float _duration = 1f;

    private float _timer;

    public void Initialize(string text, Transform parent)
    {
        transform.SetParent(parent);
        transform.localScale = Vector3.one;
        transform.localPosition = Vector3.zero;
        _text.text = text;
        _timer = 0f;
    }

    public void OnDespawn()
    {
        
    }

    public void OnSpawn()
    {
    }

    private void Update()
    {
        _timer += Time.deltaTime;
        transform.position += Vector3.up * _riseSpeed * Time.deltaTime;

        if (_timer >= _duration)
        {
            PoolManager.Instance.Despawn<HitTypePool, EHitEffectText>(EHitEffectText.FloatingHitTypeText, gameObject);
        }
    }
}
