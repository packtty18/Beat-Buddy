using TMPro;
using UnityEngine;

public class ScoreTest : MonoBehaviour
{
    [Header("Debug")]
    [SerializeField] float _addScoreAmount;
    public void AddScoreTest()
    {
        if(!ScoreManager.IsManagerExist())
        {
            return;
        }
        ScoreManager.Instance.IncreaseScore(_addScoreAmount);
    }

    public void ResetScore()
    {
        if (!ScoreManager.IsManagerExist())
        {
            return;
        }
        ScoreManager.Instance.ResetScore();
    }
}
