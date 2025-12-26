using UnityEngine;
using Unity.Netcode;

public class PlayerPresenter : NetworkBehaviour
{
    [Header("MVP Layers")]
    [SerializeField] private PlayerDataModel model;
    [SerializeField] private PlayerView view;

    [Header("Settings")]
    [SerializeField] private LayerMask solidLayer;
    [SerializeField] private GameObject bombPrefab;
    
    public IPlayerAbility _currentAbility;
    private Rigidbody2D _rb;
    private int _activeBombCount = 0;
    private float skinWidth = 0.05f;

    public override void OnNetworkSpawn()
    {
        _rb = GetComponent<Rigidbody2D>();
        _currentAbility = new BaseAbility();

        if (IsServer && model != null)
        {
            model.IsSpeedBoostActive.Value = false;
        }
    }

    void Update()
    {
        if (!IsOwner) return;
        HandleSpeedTimer();
    }

    private void HandleSpeedTimer()
    {
        if (IsServer && model.IsSpeedBoostActive.Value)
        {
            model.SpeedBoostTimer -= Time.deltaTime;
            if (model.SpeedBoostTimer <= 0) ResetSpeedServer();
        }
    }

    // Player.cs'den çağrılır
    public void RequestPlaceBomb()
    {
        PlaceBombServerRpc(_currentAbility.BombRange);
    }

    [ServerRpc]
    private void PlaceBombServerRpc(int range)
    {
        if (_activeBombCount < _currentAbility.BombCount)
        {
            Vector3 finalPos = new Vector3(Mathf.FloorToInt(transform.position.x) + 0.5f, 
                                           Mathf.FloorToInt(transform.position.y) + 0.5f, 0);
            
            GameObject bombObj = Instantiate(bombPrefab, finalPos, Quaternion.identity);
            bombObj.GetComponent<NetworkObject>().Spawn();
            
            // Bombayı kuran oyuncuyu ve menzilini ata
            Bomb bombScript = bombObj.GetComponent<Bomb>();
            if (bombScript != null)
            {
                // Buradaki 'this' PlayerPresenter'dır, Bomb.cs Player bekliyorsa GetComponent<Player>() gönder
                bombScript.Initialize(GetComponent<Player>(), range);
            }
            
            _activeBombCount++;
        }
    }

    public bool CanMove(Vector2 direction, float distance)
    {
        RaycastHit2D hit = Physics2D.BoxCast(_rb.position, Vector2.one * 0.8f, 0f, direction, distance + skinWidth, solidLayer);
        return hit.collider == null;
    }

    [ServerRpc]
    public void ApplySpeedBoostServerRpc(float duration)
    {
        model.IsSpeedBoostActive.Value = true;
        model.SpeedBoostTimer = duration;
        _currentAbility = new SpeedUpDecorator(_currentAbility);
    }

    private void ResetSpeedServer()
    {
        model.IsSpeedBoostActive.Value = false;
        _currentAbility = new BaseAbility();
    }

    public PlayerView GetView() => view;

    public void RemoveActiveBombCount() => _activeBombCount--;
}