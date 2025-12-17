
using UnityEngine;

public class Explosion : MonoBehaviour
{
    // Patlama efektinin ekranda kalma süresi (Çok kısa olmalı)
    [SerializeField] private float duration = 0.3f; 

    void Start()
    {
        // Belirlenen sürenin sonunda kendini yok et
        Destroy(gameObject, duration);
    }

    // İLERİDE EKLENECEK: Player/Enemy ile çarpışma (Trigger) kontrolü
    // void OnTriggerEnter2D(Collider2D other) { ... } 
}