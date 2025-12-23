using Unity.Netcode;
using UnityEngine;

public class PlayerDataModel : NetworkBehaviour
{
    // Herkes okuyabilir, sadece sunucu değiştirebilir (Güvenlik için)
    public NetworkVariable<float> MovementSpeed = new NetworkVariable<float>(5f);
    public NetworkVariable<int> BombCount = new NetworkVariable<int>(1);
    public NetworkVariable<int> BombRange = new NetworkVariable<int>(1);

    // Süreli efektler için (Sadece serverda tutulur)
    public float SpeedBoostTimer = 0f;
    public NetworkVariable<bool> IsSpeedBoostActive = new NetworkVariable<bool>(false);
}