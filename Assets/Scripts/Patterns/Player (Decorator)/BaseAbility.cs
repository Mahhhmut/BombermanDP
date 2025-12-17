using UnityEngine;
public class BaseAbility : IPlayerAbility
{
    // Varsayılan başlangıç değerleri
    public float MovementSpeed => 5f;
    public int BombCount => 1;
    public int BombRange => 1;

    public void Move(Rigidbody2D rb, Vector2 direction)
    {
        float distance = MovementSpeed * Time.fixedDeltaTime;
        Vector2 targetPos = rb.position + direction * distance;
        rb.MovePosition(targetPos);
    }

    public void PlaceBomb() 
    { 
        // Bu metot boş kalabilir veya temel bir log basabilir. 
        // Asıl Instantiate işlemi Player.cs içinde kalacak.
    }
}