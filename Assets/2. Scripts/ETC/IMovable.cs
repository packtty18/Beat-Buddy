using UnityEngine;

public interface IMovable
{
    public float CurrentSpeed { get; }
    public Vector2 CurrentDirection { get; }
    //다음 프레임에 움질일 방향 정의
    public void SetDirection(Vector2 direction);

    //업데이트에서 사용하는 Move
    public void Move();
}
