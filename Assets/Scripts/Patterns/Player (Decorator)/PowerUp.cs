using UnityEngine;

public enum PowerUpType { ExtraRange, ExtraBomb, SpeedBoost }

public class PowerUp : MonoBehaviour
{
    public PowerUpType type;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            ApplyPowerUp(other.gameObject);
            Destroy(gameObject);
        }
    }

    private void ApplyPowerUp(GameObject player)
    {
        Player playerScript = player.GetComponent<Player>();
        if (playerScript == null) return;

        switch (type)
        {
            case PowerUpType.ExtraRange:
                playerScript._currentAbility = new BombPowerDecorator(playerScript._currentAbility);
                break;
            case PowerUpType.ExtraBomb:
                playerScript._currentAbility = new BombCountDecorator(playerScript._currentAbility);
                break;
            case PowerUpType.SpeedBoost:
                // Dekoratör oluşturmak yerine Player içindeki süreli metodu çağırıyoruz
                playerScript.ApplySpeedBoost(10f); 
                break;
        }
        Debug.Log($"{type} uygulandı!");
    }
}