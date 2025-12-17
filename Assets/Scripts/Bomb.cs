// Bomb.cs (GameObject'e eklenecek)

using UnityEngine;
using UnityEngine.Tilemaps;

public class Bomb : MonoBehaviour
{
    // Inspector'dan atanacak ayarlar
    [SerializeField] private float timeToExplode = 3f;
    [SerializeField] private GameObject explosionPrefab; // Patlama efekti için
    [SerializeField] private LayerMask solidLayer; // Duvar katmanı (SolidMap)

    [Header("Power-Up Settings")]
    [SerializeField] private GameObject[] powerUpPrefabs; 
    [Range(0f, 1f)] [SerializeField] private float spawnChance = 0.3f;

    // Player'dan alınacak dinamik değerler
    private int _explosionRange;
    private Player _ownerPlayer; // Bombayı bırakan oyuncu

    // Timer
    private float _timer;

    public void Initialize(Player owner, int range)
    {
        _ownerPlayer = owner;
        _explosionRange = range;
        _timer = timeToExplode;
        
        
    }

    void Update()
    {
        _timer -= Time.deltaTime;
        
        if (_timer <= 0)
        {
            Explode();
        }
    }

    private void Explode()
    {
        // 1. Patlama Limitini Geri Ver
        if (_ownerPlayer != null)
        {
            _ownerPlayer.RemoveActiveBombCount(); 
        }

        Vector3 centerPos = transform.position;
        CheckDamage(centerPos);
        // 2. Patlama Merkezi (Center) Efektini Yarat
        Instantiate(explosionPrefab, centerPos, Quaternion.identity);

        // 3. Patlamayı 4 Ana Yöne Yay
        CheckExplosionDirection(centerPos, Vector2.up);
        CheckExplosionDirection(centerPos, Vector2.down);
        CheckExplosionDirection(centerPos, Vector2.right);
        CheckExplosionDirection(centerPos, Vector2.left);

        

        // Bombayı yok et
        Destroy(gameObject);
    }

    private void CheckExplosionDirection(Vector3 startPos, Vector2 direction)
    {
        for (int i = 1; i <= _explosionRange; i++)
        {
            Vector3 targetPos = startPos + (Vector3)direction * i;
            CheckDamage(targetPos);

            // Işın mesafesini tam i yaparak tam o kareye bakmasını sağlıyoruz
            RaycastHit2D hit = Physics2D.Raycast(startPos, direction, i, solidLayer);

            if (hit.collider != null)
            {
                // Eğer "Breakable" layer'ına (Kırılabilir Duvar Tilemap'i) çarptıysak
                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("BreakableMap"))
                {
                    Tilemap tilemap = hit.collider.GetComponent<Tilemap>();
                    if (tilemap != null)
                    {
                        // Duvarın tam içindeki koordinatı bul ve sil
                        Vector3Int tilePos = tilemap.WorldToCell(targetPos); 
                        tilemap.SetTile(tilePos, null); // Üstteki duvar silinir, alt katmandaki zemin görünür.

                        TrySpawnPowerUp(targetPos);
                        
                        // Duvarın patlama anını görselleştir
                        Instantiate(explosionPrefab, targetPos, Quaternion.identity);
                    }
                }
                // İster kırılsın ister kırılmasın, bir engele çarptığı için döngüden çık (Patlama durur)
                return; 
            }

            // Yol boşsa patlama görselini koy
            Instantiate(explosionPrefab, targetPos, Quaternion.identity);
        }
    }

    private void CheckDamage(Vector3 position)
    {
        // Belirlenen pozisyonda (karede) Player veya Enemy katmanında biri var mı?
        Collider2D hit = Physics2D.OverlapBox(position, Vector2.one * 0.8f, 0f, LayerMask.GetMask("Player", "Enemy"));

        if (hit != null)
        {
            Debug.Log($"<color=red>HASAR:</color> {hit.name} patlamada kaldı!");
            // İleride: hit.GetComponent<IDamageable>()?.TakeDamage();
        }
    }

    private void TrySpawnPowerUp(Vector3 position)
    {
        // 1. Şans kontrolü
        if (Random.value <= spawnChance && powerUpPrefabs.Length > 0)
        {
            // 2. Rastgele bir Power-up seç
            int randomIndex = Random.Range(0, powerUpPrefabs.Length);
            
            // 3. Oluştur
            Instantiate(powerUpPrefabs[randomIndex], position, Quaternion.identity);
        }
    }

    
}