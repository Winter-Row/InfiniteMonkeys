using System.Collections;
using UnityEngine;

public class BubbleEyeMutant : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float detectionRange = 3f;
    public Transform player;
    public Transform groundCheck, wallCheck;
    public LayerMask groundLayer;
    public int health = 2;
    public float groundCheckRadius = 0.2f;

    private Rigidbody2D rb;
    private Animator anim;
    private bool movingRight = true;
    private bool isFollowing = false;
    private bool isAttacking = false;
    private bool isPatrolling = true;
    private float patrolCooldown = 0f;
    private float patrolDistance = 0f;
    private float maxPatrolDistance = 3f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        rb.freezeRotation = true;
    }

    void Update()
    {
        float playerDistance = Vector2.Distance(transform.position, player.position);

        // Switch between following the player or patrolling based on distance
        if (playerDistance < detectionRange)
        {
            isFollowing = true;
            isPatrolling = false;
        }
        else
        {
            isFollowing = false;
            isPatrolling = true;
        }

        // Execute behavior depending on whether the enemy is following or patrolling
        if (!isAttacking)
        {
            if (isFollowing) FollowPlayer();
            else if (isPatrolling) Patrol();
        }
    }

    void Patrol()
    {
        if (Time.time < patrolCooldown) return;

        anim.SetBool("Walk", true);
        rb.velocity = new Vector2(movingRight ? moveSpeed : -moveSpeed, rb.velocity.y);
        patrolDistance += moveSpeed * Time.deltaTime;

        // Flip direction if max patrol distance is reached or if hitting an obstacle
        if (patrolDistance >= maxPatrolDistance || !IsGrounded() || IsHittingWall())
        {
            Flip();
            patrolDistance = 0;
            patrolCooldown = Time.time + 1f;
        }
    }

    void FollowPlayer()
    {
        anim.SetBool("Walk", true);
        float direction = (player.position.x > transform.position.x) ? 1 : -1;
        rb.velocity = new Vector2(direction * moveSpeed, rb.velocity.y);

        // Flip when the player changes direction
        if ((direction > 0 && !movingRight) || (direction < 0 && movingRight))
        {
            Flip();
        }
    }

    bool IsHittingWall()
    {
        return Physics2D.Raycast(wallCheck.position, Vector2.right * (movingRight ? 1 : -1), 0.3f, groundLayer);
    }

    bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }

    void Flip()
    {
        movingRight = !movingRight;
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z); // Flip the enemy's orientation
    }

    public void TakeDamage()
    {
        health--;
        if (health <= 0)
        {
            Die(); // Trigger death if health reaches 0
        }
    }

    void Die()
    {
        anim.SetTrigger("Death");
        rb.velocity = Vector2.zero;
        rb.isKinematic = true; // Disable physics interaction after death
        GetComponent<Collider2D>().enabled = false; // Disable collisions after death
        Destroy(gameObject, 1.5f); // Destroy the enemy after a delay
    }

    void OnDrawGizmosSelected()
    {
        // Visualize ground check radius
        if (groundCheck != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}
// Behavior summary:
// The BubbleEyeMutant patrols an area until it detects the player. Once detected, it switches to following the player.
// It attacks the player at regular intervals and changes direction after a set number of attacks for variety.
// If it hits an obstacle or reaches the end of its patrol area, it flips direction and resumes patrolling.
