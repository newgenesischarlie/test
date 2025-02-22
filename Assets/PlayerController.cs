using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Speed of the player
    public float moveSpeed = 5f;

    // Rigidbody2D reference for 2D physics-based movement
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        // Get the Rigidbody2D component attached to the player
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // Get player movement input
        float moveX = Input.GetAxisRaw("Horizontal"); // -1 for left, 1 for right
        float moveY = Input.GetAxisRaw("Vertical");   // -1 for down, 1 for up

        // Calculate the movement vector
        Vector2 moveDirection = new Vector2(moveX, moveY).normalized;

        // Apply the movement to the player's Rigidbody2D
        rb.velocity = moveDirection * moveSpeed;

        Debug.Log("Player position: " + transform.position);

    }
}
