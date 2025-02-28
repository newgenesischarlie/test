using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        // Get the Animator component attached to the player
        animator = GetComponent<Animator>();
    }

    public void SetWalkingAnimation(bool isWalking)
    {
        // Set the 'isWalking' parameter in the Animator to control the walking animation
        animator.SetBool("isWalking", isWalking);
    }

    public void FlipSpriteDirection(float moveX)
    {
        // Flip the sprite's direction based on movement
        if (moveX > 0)
        {
            transform.localScale = new Vector3(1, 1, 1); // Face right
        }
        else if (moveX < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1); // Face left
        }
    }
}
