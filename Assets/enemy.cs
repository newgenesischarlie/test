using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    //Define the necessary variables
    public float MoveSpeed = 3f; // Move speed
    private int _currentWaypointIndex = 0; // Index to track the current waypoint
    private Vector3 _lastPointPosition; // Last position of the enemy
    private SpriteRenderer _spriteRenderer; // To access the sprite renderer for flipping
                                            //  public Vector3 CurrentPointPosition => Waypoint.Points[_currentWaypointIndex]; // The current waypoint position
    private EnemyHealth _enemyHealth; // Enemy health script
    public delegate void EndReachedHandler(EnemyMove enemy);
    public event EndReachedHandler OnEndreached; // Event for when the endpoint is reached

    void Start()
    {
        // Initialize the sprite renderer and enemy health
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _enemyHealth = GetComponent<EnemyHealth>();
    }

    private void Update()
    {
        Move();
        Rotate();
        if (CurrentPointPositionReached())
        {
            UpdateCurrentPointIndex();
        }
    }

    private void Move()
    {
        // Move the enemy towards the current waypoint
        transform.position = Vector3.MoveTowards(transform.position, CurrentPointPosition, MoveSpeed * Time.deltaTime);
    }

    private void Rotate()
    {
        // Rotate the sprite based on the direction
        if (transform.position.x < CurrentPointPosition.x)
        {
            _spriteRenderer.flipX = false;
        }
        else
        {
            _spriteRenderer.flipX = true;
        }
    }

    private bool CurrentPointPositionReached()
    {
        // Check if the enemy has reached the current waypoint
        float distanceToNextPointPosition = (transform.position - CurrentPointPosition).magnitude;
        if (distanceToNextPointPosition < 0.1f)
        {
            _lastPointPosition = transform.position;
            return true;
        }

        return false;
    }

    private void UpdateCurrentPointIndex()
    {
        // Update the waypoint index when the current waypoint is reached
        int lastWaypointIndex = Waypoint.Points.Length - 1;
        if (_currentWaypointIndex < lastWaypointIndex)
        {
            _currentWaypointIndex++;
        }
        else
        {
            EndPointReached();
        }
    }

    private void EndPointReached()
    {
        // When the endpoint is reached, invoke the event and reset the enemy
        OnEndreached?.Invoke(this);
        _enemyHealth.ResetHealth();
        ObjectPooler.ReturnToPool(gameObject); // Return the enemy to the object pool
    }
}
