using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private IEnemyState _currentState;
    private Rigidbody2D _rb;
    
    [SerializeField] public float speed = 2f;
    [SerializeField] private LayerMask solidLayer;
    private float skinWidth = 0.05f;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _rb.gravityScale = 0;
        _rb.freezeRotation = true;
    }

    void Start() => _currentState = new PatrolState();

    void FixedUpdate()
    {
        _currentState?.UpdateState(this);
    }

    public void MovePhysical(Vector2 direction)
    {
        if (direction == Vector2.zero) return;

        float dist = speed * Time.fixedDeltaTime;

        // Player'daki CanMove mantığının aynısı
        if (CanMove(direction, dist))
        {
            Vector2 nextPos = _rb.position + direction * dist;
            _rb.MovePosition(nextPos);
        }
    }

    public bool CanMove(Vector2 direction, float distance)
    {
        Collider2D col = GetComponent<Collider2D>();
        RaycastHit2D hit = Physics2D.BoxCast(
            col.bounds.center, 
            col.bounds.size * 0.9f, 
            0f, 
            direction, 
            distance + skinWidth, 
            solidLayer
        );
        return hit.collider == null;
    }

    public void TransitionToState(IEnemyState newState) => _currentState = newState;
}