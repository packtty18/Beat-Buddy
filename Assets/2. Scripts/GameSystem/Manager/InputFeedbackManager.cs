using UnityEngine;

public class InputFeedbackManager : SimpleSingleton<InputFeedbackManager>
{
    [Header("타격 피드백")]
    [SerializeField] private InputFeedback _leftFeedback;
    [SerializeField] private InputFeedback _rightFeedback;

    public void TriggerLeftFeedback()
    {
        if (_leftFeedback != null)
        {
            _leftFeedback.Trigger();
        }
    }

    public void TriggerRightFeedback()
    {
        if (_rightFeedback != null)
        {
            _rightFeedback.Trigger();
        }
    }

    public void TriggerFeedback(ENoteType noteType)
    {
        if (noteType == ENoteType.LNote)
            TriggerLeftFeedback();
        else
            TriggerRightFeedback();
    }
}
