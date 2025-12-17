// RandomMovement.cs
using UnityEngine;
using System.Collections.Generic;

public class RandomMovement : IMovementStrategy
{
    private float _changeDirectionTime = 1.5f; 
    private float _timer;
    private Vector2 _currentDirection;
    private MapManager _mapManager; // Harita verisine ulaşmak için

    private readonly Vector2[] possibleDirections = { Vector2.up, Vector2.down, Vector2.left, Vector2.right };

    public RandomMovement(float changeTime)
    {
        _changeDirectionTime = changeTime;
        _timer = 0f;
        _mapManager = Object.FindFirstObjectByType<MapManager>();
    }

    // IMovementStrategy uygulama
    public void Move(Transform enemyTransform, Rigidbody2D rb, float speed)
    {
        _timer -= Time.fixedDeltaTime;

        if (_timer <= 0 && _mapManager != null)
        {
            ChangeRandomDirection(enemyTransform);
            _timer = _changeDirectionTime; 
        }

        if (_currentDirection != Vector2.zero)
        {
            // Rigidbody ile fiziksel hareket
            Vector2 newPosition = rb.position + _currentDirection * speed * Time.fixedDeltaTime;
            rb.MovePosition(newPosition); 
        }
    }
    
    // IMovementStrategy uygulama
    public void CollisionDetected(Vector2 collisionNormal)
    {
        // Çarpışma anında hemen yön değiştirmeyi zorla
        _timer = 0f; 
    }

    // Yürünebilir rastgele bir yön seçer
    private void ChangeRandomDirection(Transform enemyTransform)
    {
        // ... (Harita kontrol ve yön seçme mantığı buraya gelir)
        // ... (En son paylaşılan RandomMovement.cs içindeki mantığı kullanın)
    }
    
    // WorldToGrid metodu da RandomMovement içinde olmalı
    private Vector2Int WorldToGrid(Vector3 worldPosition)
    {
        // ... (Yuvarlama mantığı)
        return new Vector2Int(Mathf.RoundToInt(worldPosition.x), Mathf.RoundToInt(worldPosition.y));
    }
}