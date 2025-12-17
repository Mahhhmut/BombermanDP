// IMovementStrategy.cs
using UnityEngine;

public interface IMovementStrategy 
{
    // Hareketi Rigidbody2D kullanarak yapar (bu yüzden 3 argüman almalı)
    void Move(Transform enemyTransform, Rigidbody2D rb, float speed); 
    
    // Düşmanın bir engele çarptığını stratejiye bildirir
    void CollisionDetected(Vector2 collisionNormal); 
}