using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    private Animator _animator;
    private Rigidbody2D _rb;

    void Awake()
    {
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Girdileri doğrudan Input'tan alalım
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        UpdateAnimations(moveX, moveY);
    }

    private void UpdateAnimations(float x, float y)
    {
        if (_animator == null) return;

        // Hareket varsa yönü güncelle
        if (x != 0 || y != 0)
        {
            _animator.SetFloat("x", x);
            _animator.SetFloat("y", y);
           
        }
        
    }
}