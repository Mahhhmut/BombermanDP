using UnityEngine;
using Unity.Netcode;

public class Player : NetworkBehaviour
{
    private PlayerPresenter _presenter;
    private Rigidbody2D _rb;
    private Vector2 _moveDirection;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _presenter = GetComponent<PlayerPresenter>();
    }

    void Update()
    {
        if (!IsOwner) return;

        // Hareket Girdisi
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        
        if (Mathf.Abs(moveX) > 0) _moveDirection = new Vector2(moveX, 0).normalized;
        else if (Mathf.Abs(moveY) > 0) _moveDirection = new Vector2(0, moveY).normalized;
        else _moveDirection = Vector2.zero;

        // Bomba Bırakma
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _presenter.RequestPlaceBomb();
        }
    }

    void FixedUpdate()
    {
        if (!IsOwner || _moveDirection == Vector2.zero) return;

        // Presenter üzerinden hızı ve hareket iznini kontrol et
        float speed = _presenter._currentAbility.MovementSpeed;
        float dist = speed * Time.fixedDeltaTime;

        if (_presenter.CanMove(_moveDirection, dist))
        {
            _presenter._currentAbility.Move(_rb, _moveDirection);
        }
    }
}