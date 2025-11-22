using UnityEngine;
using UnityEngine.UI;

public enum ELightningPatternType
{
    Lightning,
    ThunderArrow,
    Thunder
}


public class LightningBoss : MonoBehaviour
{
    [Header("패턴 프리팹")]
    [SerializeField] private GameObject[] _lightningPrefab;
    private GameObject _lightning;


    [Header("라이트닝이펙트 포지션")]
    [SerializeField] private Transform LightningPosition1;
    [SerializeField] private Transform LightningPosition2;
    [SerializeField] private Transform LightningPosition3;


    private void Start()
    {
        FlashScreen.Flash();
        StartAttackEffect();
    }

    private void StartAttackEffect()
    {
        _lightning = Instantiate(_lightningPrefab[(int)ELightningPatternType.Lightning], LightningPosition1);
        _lightning = Instantiate(_lightningPrefab[(int)ELightningPatternType.Lightning], LightningPosition2);
        _lightning = Instantiate(_lightningPrefab[(int)ELightningPatternType.Lightning], LightningPosition3);
    }
}