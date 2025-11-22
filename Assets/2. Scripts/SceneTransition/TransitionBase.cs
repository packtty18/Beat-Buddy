using System.Collections;
using UnityEngine;

public abstract class TransitionBase : MonoBehaviour, ISceneTransition
{
    //연출에 필요한 객체 지정

    //연출에 대한 기본 설정

    public IEnumerator PlayTransition()
    {
        throw new System.NotImplementedException();
    }
}
