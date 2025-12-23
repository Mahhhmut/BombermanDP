using UnityEngine;

public class PlayerView : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;

    public void UpdateAnimation(Vector2 direction)
    {
        bool moving = direction.sqrMagnitude > 0.01f;
        
        // Animator'daki isMoving parametresini güncelle
        animator.SetBool("isMoving", moving);

        if (moving)
        {
            // Sadece yürürken yön parametrelerini gönder
            animator.SetFloat("MoveX", direction.x);
            animator.SetFloat("MoveY", direction.y);
        }
    }
}