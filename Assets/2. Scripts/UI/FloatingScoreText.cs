using UnityEngine;
using TMPro;

public class FloatingScoreText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private float _riseSpeed = 50f;
    [SerializeField] private float _duration = 1f;

    private float _timer;

    public void Initialize(string text)
    {
        _text.text = text;
        _timer = 0f;
    }

    private void Update()
    {
        _timer += Time.deltaTime;
        transform.position += Vector3.up * _riseSpeed * Time.deltaTime;

        if (_timer >= _duration)
        {
            Destroy(gameObject);
        }
    }
}
