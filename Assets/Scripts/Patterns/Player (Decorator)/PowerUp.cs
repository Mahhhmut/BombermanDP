using UnityEngine;
using Unity.Netcode;

public enum PowerUpType { ExtraRange, ExtraBomb, SpeedBoost }

public class PowerUp : NetworkBehaviour
{
    public PowerUpType type;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Sadece Server temasları işler ve objeyi ağdan siler
        if (!IsServer) return;

        if (other.CompareTag("Player"))
        {
            ApplyPowerUp(other.gameObject);
            // Objeyi ağdaki tüm clientlardan sil
            GetComponent<NetworkObject>().Despawn();
        }
    }

    private void ApplyPowerUp(GameObject playerObj)
    {
        var presenter = playerObj.GetComponent<PlayerPresenter>();
        if (presenter == null) return;

        switch (type)
        {
            case PowerUpType.ExtraRange:
                presenter._currentAbility = new BombPowerDecorator(presenter._currentAbility);
                break;
            case PowerUpType.ExtraBomb:
                presenter._currentAbility = new BombCountDecorator(presenter._currentAbility);
                break;
            case PowerUpType.SpeedBoost:
                // Presenter üzerindeki ServerRpc'yi çağırarak süreli hızı başlat
                presenter.ApplySpeedBoostServerRpc(10f); 
                break;
        }
        Debug.Log($"{type} sunucu tarafından {playerObj.name} oyuncusuna uygulandı!");
    }
}