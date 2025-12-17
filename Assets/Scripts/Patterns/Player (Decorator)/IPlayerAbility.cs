using UnityEngine;

public interface IPlayerAbility
{
    float MovementSpeed { get; }
    int BombCount { get; }
    int BombRange { get; }

    //bu metod oyuncu hareketini FixedUpdate içinde çağırır.
    void Move(Rigidbody2D rb, Vector2 direction);
    void PlaceBomb();
}
