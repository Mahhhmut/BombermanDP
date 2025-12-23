using UnityEngine;

public class Player : MonoBehaviour
{
    public IPlayerAbility _currentAbility; // Decorator zinciri
    private Rigidbody2D _rb;
    private Vector2 _moveDirection;
    private int _activeBombCount = 0;
    private bool _isSpeedBoostActive = false;
    private float _speedBoostTimer = 0;
    private IPlayerAbility _preSpeedAbility; // Hızlanmadan önceki yetenek zinciri

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

        if (_isSpeedBoostActive)
        {
            _speedBoostTimer -= Time.deltaTime;
            if (_speedBoostTimer <= 0)
            {
                ResetSpeed();
            }
        }

        // Bomba Bırakma
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PlaceBomb();
        }
    }

    void FixedUpdate()
    {
        if (_moveDirection != Vector2.zero)
        {
            float speed = _currentAbility.MovementSpeed;
            float dist = speed * Time.fixedDeltaTime;



            if (CanMove(_moveDirection, dist))
            {
                // Decorator zincirindeki Move metodunu çağırıyoruz
                _currentAbility.Move(_rb, _moveDirection);
            }
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Düşmanın Tag'ının "Enemy" olduğundan emin ol
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Öldün!");
            // Sahneyi yeniden başlat
            UnityEngine.SceneManagement.SceneManager.LoadScene(
                UnityEngine.SceneManagement.SceneManager.GetActiveScene().name
            );
        }
    }

    public void ApplySpeedBoost(float duration)
    {
        // Eğer zaten hız artışı varsa sadece süreyi yenile (Stacklenmeyi engeller)
        if (_isSpeedBoostActive)
        {
            _speedBoostTimer = duration;
            return;
        }

        // İlk kez hızlanıyorsa:
        _isSpeedBoostActive = true;
        _speedBoostTimer = duration;
        
        // Mevcut yetenek zincirini sakla ve üzerine hız dekoratörünü ekle
        _currentAbility = new SpeedUpDecorator(_currentAbility);
        Debug.Log("Hız artışı başladı!");
    }

    private void ResetSpeed()
    {
        _isSpeedBoostActive = false;
        
        // Decorator zincirini manuel olarak temizlemek yerine 
        // Basitçe hızı normalleştiren bir mantık veya zinciri yeniden kurma:
        // En temiz yol: Mevcut yetenekleri koruyup sadece hızı Base seviyeye çekmek
        // Veya bu örnekte olduğu gibi objeyi yeniden BaseAbility'e döndürmek (diğer power-up'ları korumak istiyorsan zinciri taraman gerekir)
        
        _currentAbility = new BaseAbility(); 
        Debug.Log("Hız normale döndü.");
    }
}