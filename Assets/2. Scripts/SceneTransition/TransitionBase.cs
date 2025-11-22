using System.Collections;
using UnityEngine;

public abstract class TransitionBase : MonoBehaviour, ISceneTransition
{
    //로딩창을 인아웃 하는 데 필요한 연출 스크립트의 기본 클래스
    //연출에 필요한 객체 지정

    //연출에 대한 기본 설정

    public IEnumerator PlayTransition()
    {
        throw new System.NotImplementedException();
    }
}
