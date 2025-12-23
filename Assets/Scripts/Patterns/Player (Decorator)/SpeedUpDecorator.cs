using UnityEngine;

public class SpeedUpDecorator : PlayerAbilityDecorator
{
    public SpeedUpDecorator(IPlayerAbility decoratedAbility) : base(decoratedAbility) { }

    // Üst sınıftaki (veya bir önceki dekoratördeki) hızı 2 ile çarpar
    public override float MovementSpeed => _decoratedAbility.MovementSpeed * 2f;

    public override void Move(Rigidbody2D rb, Vector2 direction)
    {
        // Hareket ederken kendi (güncel) MovementSpeed değerini kullanır
        float distance = MovementSpeed * Time.fixedDeltaTime;
        Vector2 targetPos = rb.position + direction * distance;
        rb.MovePosition(targetPos);
    }
}
