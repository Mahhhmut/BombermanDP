using UnityEngine;

public class Player : MonoBehaviour
{
    public IPlayerAbility _currentAbility; // Decorator zinciri
    private Rigidbody2D _rb;
    private Vector2 _moveDirection;
    private int _activeBombCount = 0;

    [Header("Ayarlar")]
    [SerializeField] private LayerMask solidLayer;
    [SerializeField] private GameObject bombPrefab;
    private float skinWidth = 0.05f;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        // Zinciri BaseAbility ile başlatıyoruz
        _currentAbility = new BaseAbility(); 
    }

    void Update()
    {
        // Hareket Girdisi
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        
        if (Mathf.Abs(moveX) > 0) _moveDirection = new Vector2(moveX, 0).normalized;
        else if (Mathf.Abs(moveY) > 0) _moveDirection = new Vector2(0, moveY).normalized;
        else _moveDirection = Vector2.zero;

        // Bomba Bırakma
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PlaceBomb();
        }
    }

    void FixedUpdate()
    {
        float speed = _currentAbility.MovementSpeed;
        float dist = speed * Time.fixedDeltaTime;

        if (CanMove(_moveDirection, dist))
        {
            _currentAbility.Move(_rb, _moveDirection);
        }
    }

    public void PlaceBomb()
    {
        if (_activeBombCount < _currentAbility.BombCount)
        {
            // Izgaraya (Grid) hizalama
            float finalX = Mathf.FloorToInt(transform.position.x) + 0.5f;
            float finalY = Mathf.FloorToInt(transform.position.y) + 0.5f;
            Vector3 finalBombPos = new Vector3(finalX, finalY, 0);

            GameObject bombObj = Instantiate(bombPrefab, finalBombPos, Quaternion.identity);
            
            Bomb bombScript = bombObj.GetComponent<Bomb>();
            if (bombScript != null)
            {
                bombScript.Initialize(this, _currentAbility.BombRange); 
            }
            
            _activeBombCount++;
        }
    }

    public void RemoveActiveBombCount() => _activeBombCount--;

    private bool CanMove(Vector2 direction, float distance)
    {
        if (direction == Vector2.zero) return true;

        Collider2D playerCollider = GetComponent<Collider2D>();
        RaycastHit2D hit = Physics2D.BoxCast(
            playerCollider.bounds.center, 
            playerCollider.bounds.size * 0.9f, 
            0f, 
            direction, 
            distance + skinWidth, 
            solidLayer
        );
        return hit.collider == null;
    }

    public void ApplyDecorator(IPlayerAbility newAbility)
    {
        _currentAbility = newAbility;
        Debug.Log($"Yetenek Güncellendi! Hız: {_currentAbility.MovementSpeed}, Menzil: {_currentAbility.BombRange}, Bomba: {_currentAbility.BombCount}");
    }
}