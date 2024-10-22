using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float moveSpeed = 2f;           // Enemy's movement speed
    public float patrolRange = 5f;         // Distance for patrolling
    public float chaseSpeed = 4f;          // Speed when chasing the player
    public float detectionRange = 3f;      // Range within which enemy detects the player

    private bool movingRight = true;       // Direction of patrolling
    private float patrolStartX;            // Starting X position for patrolling
    private Rigidbody2D rb;                // Enemy's Rigidbody2D component
    private Transform player;              // Reference to the player's Transform
    public class PlayerHealth : MonoBehaviour
    {
        public int maxHealth = 100; // Maximum health value
        private int currentHealth;  // Current health value

        void Start()
        {
            currentHealth = maxHealth;  // Set the player's initial health
        }

        public void TakeDamage(int damage)
        {
            currentHealth -= damage;  // Reduce current health by damage amount
            Debug.Log("Player took damage, current health: " + currentHealth);

            if (currentHealth <= 0)
            {
                Die();
            }
        }

        void Die()
        {
            Debug.Log("Player has died!");
            // Add death behavior here, such as triggering a game over screen
        }
    }
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        patrolStartX = transform.position.x;  // Set patrol starting point
        player = GameObject.FindGameObjectWithTag("Player").transform;  // Find the player using tag
    }

    void Update()
    {
        // Calculate distance between the enemy and the player
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // If the player is within the detection range, chase the player
        if (distanceToPlayer <= detectionRange)
        {
            ChasePlayer();
        }
        else
        {
            Patrol();
        }
    }

    // Patrolling movement
    void Patrol()
    {
        // Check if the enemy is moving beyond patrol range and reverse direction
        if (movingRight && transform.position.x >= patrolStartX + patrolRange)
        {
            Flip();
        }
        else if (!movingRight && transform.position.x <= patrolStartX - patrolRange)
        {
            Flip();
        }

        // Move the enemy
        rb.velocity = new Vector2((movingRight ? moveSpeed : -moveSpeed), rb.velocity.y);
    }

    // Chase the player when they are within detection range
    void ChasePlayer()
    {
        // Move towards the player
        float direction = player.position.x > transform.position.x ? 1 : -1;
        rb.velocity = new Vector2(direction * chaseSpeed, rb.velocity.y);

        // Flip enemy direction to face the player
        if ((direction > 0 && !movingRight) || (direction < 0 && movingRight))
        {
            Flip();
        }
    }

    // Flip the enemy's direction (left or right)
    void Flip()
    {
        movingRight = !movingRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }

    // Optional: Visualize patrol and detection range in Unity editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(new Vector2(patrolStartX - patrolRange, transform.position.y), new Vector2(patrolStartX + patrolRange, transform.position.y));

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
public class Bullet : MonoBehaviour
{
    public int damageAmount = 10;  // Damage dealt to the enemy upon collision

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the bullet collides with an object tagged as "Enemy"
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // Get the enemy's health component (assuming there's a script named EnemyAI)
            EnemyAI enemyAI = collision.gameObject.GetComponent<EnemyAI>();
            if (enemyAI != null)
            {
                // Deal damage to the enemy
                enemyAI.TakeDamage(damageAmount);
            }

            // Destroy the bullet after it hits the enemy
            Destroy(gameObject);
        }
        else
        {
            // Optionally destroy the bullet if it collides with something else
            Destroy(gameObject);
        }
    }
}
