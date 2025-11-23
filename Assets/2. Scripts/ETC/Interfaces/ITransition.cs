using System.Collections;

public interface ITransition
{
    IEnumerator PlayOut();
    IEnumerator PlayIn();
}
