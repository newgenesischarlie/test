using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody2D rb;
   // private Animation playerAnimation;  // Reference to the Animation component

    void Start()
    {
        // Get the Rigidbody2D and Animation components attached to the player
        rb = GetComponent<Rigidbody2D>();
       /// playerAnimation = GetComponent<Animation>();

        // Ensure Animation was found
      //  if (playerAnimation == null)
      //  {
      //      Debug.LogError("Animation component not found on this GameObject!");
      //  }

        // Set the Rigidbody2D to Kinematic to ensure manual movement with script
        rb.bodyType = RigidbodyType2D.Kinematic;
    }

    void Update()
    {
        // Ensure playerAnimation is assigned before using it
      // if (playerAnimation != null)
        {
            // Get player movement input
            float moveX = Input.GetAxisRaw("Horizontal");
            float moveY = Input.GetAxisRaw("Vertical");

            // Calculate the movement vector
            Vector2 moveDirection = new Vector2(moveX, moveY).normalized;

            // Apply the movement to the player's Rigidbody2D
            rb.velocity = moveDirection * moveSpeed;

            // Update the animation based on player movement
          //  if (moveDirection.magnitude > 0)
          //  {
                // Play the walking animation if moving
          //      if (!playerAnimation.isPlaying) // Ensure it's not already playing
          //      {
          //          playerAnimation.Play("Walk");  // Name of your legacy walking animation clip
          //      }
          //  }
          //  else
         //   {
          //      // Optionally stop the walking animation if not moving
           //     if (playerAnimation.isPlaying)
           //     {
           //         playerAnimation.Stop();  // Stops the animation when not moving
           //     }
          //  }

            // Flip the sprite direction based on horizontal movement
           // FlipSpriteDirection(moveX);

          //  // Debugging the player's position
          //  Debug.Log("Player position: " + transform.position);
       // }
       // else
      //  {
      //      Debug.LogWarning("Animation component is not assigned.");
      //  }
    }

    void FlipSpriteDirection(float moveX)
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
}
