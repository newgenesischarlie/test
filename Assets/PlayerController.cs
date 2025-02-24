using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody2D rb;

    void Start()
    {
        // Get the Rigidbody2D component attached to the player
        rb = GetComponent<Rigidbody2D>();

        // Set the Rigidbody2D to Kinematic to ensure manual movement with script
        rb.bodyType = RigidbodyType2D.Kinematic;
    }

    void Update()
    {
        // Get player movement input
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        // Calculate the movement vector
        Vector2 moveDirection = new Vector2(moveX, moveY).normalized;

        // Apply the movement to the player's Rigidbody2D
        rb.velocity = moveDirection * moveSpeed;

        // Debugging the player's position
        Debug.Log("Player position: " + transform.position);
    }
}
