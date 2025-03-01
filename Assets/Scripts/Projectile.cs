using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BasicProjectile : MonoBehaviour
{
    public static Action<Enemy, float> OnEnemyHit;
    [SerializeField] private float speed = 10f;
    [SerializeField] private int damage = 10;
    [SerializeField] private float lifetime = 3f;
    [SerializeField] private string targetTag = "Enemy";

    private Vector3 direction;
    private float timer;

    private void OnEnable()
    {
        timer = 0f;
        direction = transform.forward;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= lifetime)
        {
            ReturnToPool();
            return;
        }

        // Move in the current direction
        transform.position += direction * speed * Time.deltaTime;
    }

    public void SetDirection(Vector3 direction)
    {
        this.direction = direction.normalized;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(targetTag))
        {
            EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage);
            }
            ReturnToPool();
        }
    }

    private void ReturnToPool()
    {
        ObjectPooler.ReturnToPool(gameObject);
    }
}
