using UnityEngine;

public interface IMove
{
    void OnMove(Vector2 move);

    void OnJump();
}

public interface IAttack
{
    void OnAttackPressed();
    void OnAttackReleased();
}