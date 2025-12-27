using UnityEngine;
using Unity.Netcode;
using System.Collections;

public class Bomb : NetworkBehaviour
{
    [SerializeField] private float timeToExplode = 3f;
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private LayerMask solidLayer;
    [SerializeField] private LayerMask BreakableLayer;
    [SerializeField] private GameObject[] powerUpPrefabs;
    [Range(0f, 1f)] [SerializeField] private float spawnChance = 0.3f;

    private int _explosionRange;
    private Player _ownerPlayer;

    public void Initialize(Player owner, int range)
    {
        _ownerPlayer = owner;
        _explosionRange = range;
    }

    public override void OnNetworkSpawn()
    {
        if (IsServer) StartCoroutine(ExplodeAfterDelay());
    }

    private IEnumerator ExplodeAfterDelay()
    {
        yield return new WaitForSeconds(timeToExplode);
        Explode();
    }

    private void Explode()
    {
        if (!IsServer) return;

        if (_ownerPlayer != null) _ownerPlayer.GetComponent<PlayerPresenter>().RemoveActiveBombCount();

        Vector3 centerPos = transform.position;
        SpawnExplosionEffect(centerPos);
        CheckDamage(centerPos);

        CheckDirection(centerPos, Vector2.up);
        CheckDirection(centerPos, Vector2.down);
        CheckDirection(centerPos, Vector2.right);
        CheckDirection(centerPos, Vector2.left);

        GetComponent<NetworkObject>().Despawn();
    }

    private void CheckDirection(Vector3 startPos, Vector2 direction)
    {
        for (int i = 1; i <= _explosionRange; i++)
        {
            Vector3 targetPos = startPos + (Vector3)direction * i;
            RaycastHit2D hit = Physics2D.Raycast(startPos, direction, i, solidLayer);

            if (hit.collider != null)
            {
                if (hit.collider.CompareTag("Breakable"))
                {
                    // Duvarı ağ üzerinde yok et
                    var netObj = hit.collider.GetComponent<NetworkObject>();
                    if (netObj != null) netObj.Despawn();
                    
                    TrySpawnPowerUp(targetPos);
                    SpawnExplosionEffect(targetPos);
                }
                return; // Engele çarptı, patlama durur
            }
            SpawnExplosionEffect(targetPos);
            CheckDamage(targetPos);
        }
    }

    private void SpawnExplosionEffect(Vector3 pos)
    {
        GameObject fx = Instantiate(explosionPrefab, pos, Quaternion.identity);
        fx.GetComponent<NetworkObject>().Spawn();
    }

    private void CheckDamage(Vector3 pos)
{
    if (!IsServer) return; // Yetki kontrolü

    // SolidMap, Player ve Enemy katmanlarını tara
    Collider2D hit = Physics2D.OverlapBox(pos, Vector2.one * 0.8f, 0f, 
        LayerMask.GetMask("SolidMap", "Player", "Enemy", "BreakableMap"));
    
    if (hit != null)
    {
        // 1. Tag kontrolü: Sadece "Breakable" olanları yok et
        if (hit.CompareTag("Breakable"))
        {
            var netObj = hit.GetComponent<NetworkObject>();
            if (netObj != null && netObj.IsSpawned) 
                netObj.Despawn(true); // Ağdan sil ve objeyi yok et
        }
        // 2. Düşman kontrolü
        else if (hit.CompareTag("Enemy"))
        {
            var netObj = hit.GetComponent<NetworkObject>();
            if (netObj != null) netObj.Despawn(true);
        }
        // 3. Oyuncu kontrolü
        else if (hit.CompareTag("Player"))
        {
            hit.GetComponent<PlayerPresenter>().DieServerRpc();
        }
        
        // Not: Eğer çarptığı şeyin Tag'ı "Breakable" değilse (Kalıcı duvarsa),
        // kod burayı pas geçer ve kalıcı duvara bir şey olmaz.
    }
}

    private void TrySpawnPowerUp(Vector3 pos)
    {
        // Sadece sunucu spawn yapabilir ve liste boş olmamalı
        if (!IsServer || powerUpPrefabs == null || powerUpPrefabs.Length == 0) return;

        if (Random.value <= spawnChance)
        {
            int index = Random.Range(0, powerUpPrefabs.Length);
            
            // Seçilen prefab null mı kontrol et
            if (powerUpPrefabs[index] == null) return;

            GameObject pu = Instantiate(powerUpPrefabs[index], pos, Quaternion.identity);
            
            // Power-up üzerinde NetworkObject var mı kontrol et
            if (pu.TryGetComponent<NetworkObject>(out var netObj))
            {
                netObj.Spawn();
            }
        }
    }

    
}