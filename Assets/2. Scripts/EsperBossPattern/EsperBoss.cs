using UnityEngine;

public class EsperBoss : MonoBehaviour
{
    [Header("S자 이동 관련 옵션")]
    private float _noteAmplitude;  // 진폭
    private float _noteFrequency;  // 흔들리는 빈도

    [Header("진폭 랜덤값")]
    private float _minAmplitude = 0.8f;
    private float _maxAmplitude = 1.6f;

    [Header("빈도 랜덤값")]
    private float _minFrequency = 3f;
    private float _maxFrequency = 6f;


    [Header("노트 위치")]
    private Vector2 _noteOriginPosition;
    private Vector2 _noteCurrentPosition;


    private void Start()
    {
        _noteOriginPosition = transform.position;

        _noteAmplitude = Random.Range(_minAmplitude, _maxAmplitude); // 진폭 랜덤값 지정
        _noteFrequency = Random.Range(_minFrequency, _maxFrequency); // 빈도 랜덤값 지정
    }

    private void Update()
    {
        NoteSMoving();
    }

    // S자 이동 메서드
    private void NoteSMoving()
    {
        _noteCurrentPosition = transform.position;
        _noteCurrentPosition.y = _noteOriginPosition.y + Mathf.Sin(Time.time * _noteFrequency) * _noteAmplitude;

        transform.position = _noteCurrentPosition;
    }
}
