// Enemy.cs
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float speed = 3f;
    
    private IMovementStrategy _movementStrategy;
    private Rigidbody2D _rb; 
    private SpriteRenderer _spriteRenderer;

    void Start()
    {
        // 1. Gerekli Bileşenleri Al
        _rb = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();

        if (_rb == null || _spriteRenderer == null)
        {
            Debug.LogError("Enemy GameObject'inde Rigidbody2D veya SpriteRenderer bileşeni eksik!");
            enabled = false;
            return;
        }

        // 2. Stratejiyi Başlat
        _movementStrategy = new RandomMovement(2f); 

        // 3. Görsel Sıralama Layer'ını Ayarla
        // (Unity'de "Enemies" Sorting Layer'ı oluşturulmuş olmalı)
        _spriteRenderer.sortingLayerName = "Enemies"; 
    }

    // Y-Sıralama (Daha aşağıdakinin önde görünmesi)
    void Update()
    {
        _spriteRenderer.sortingOrder = Mathf.RoundToInt(transform.position.y * -100); 
    }
    
    // Fiziksel Hareket (MovePosition FixedUpdate'te olmalı)
    void FixedUpdate()
    {
        if (_movementStrategy != null)
        {
            // HAREKET: Stratejiyi çağır ve gerekli 3 argümanı gönder
            _movementStrategy.Move(transform, _rb, speed);
        }
    }
    
    // Çarpışma Tespiti (Tilemap Collider 2D'ye çarpınca)
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Harita engelleri ile çarpıştığından emin ol
        if (collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Breakable"))
        {
            if (_movementStrategy != null)
            {
                // Çarpışma bilgisini stratejiye ilet (yeni yön seçmesi için)
                Vector2 normal = collision.contacts[0].normal;
                _movementStrategy.CollisionDetected(normal);
            }
        }
    }
}