using Unity.Netcode;
using UnityEngine;

public class NetworkDestroyer : NetworkBehaviour
{
    [SerializeField] private float lifeTime = 1f;

    public override void OnNetworkSpawn()
    {
        // Sadece sunucu zamanlayıcıyı başlatır ve objeyi ağdan siler
        if (IsServer)
        {
            Invoke(nameof(DespawnObject), lifeTime);
        }
    }

    private void DespawnObject()
    {
        if (IsSpawned)
        {
            GetComponent<NetworkObject>().Despawn();
        }
    }
}