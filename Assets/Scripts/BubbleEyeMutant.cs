using System.Collections;
using UnityEngine;

public class BubbleEyeMutant : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float detectionRange = 3f;
    public Transform player;
    public Transform attackPoint;
    public Transform groundCheck, wallCheck;
    public LayerMask groundLayer;
    public int health = 2;
    public float groundCheckRadius = 0.2f;
    public float attackRange = 2.5f;

    private Rigidbody2D rb;
    private Animator anim;
    private bool movingRight = true;
    private bool isFollowing = false;
    private bool isAttacking = false;
    private bool isPatrolling = true;
    private float patrolCooldown = 0f;
    private float patrolDistance = 0f;
    private float maxPatrolDistance = 3f;
    private int attackCount = 0;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        rb.freezeRotation = true;

        InvokeRepeating(nameof(Attack), 3f, 3f); // Attack every 3 seconds

        // Call Die() after 5 seconds to simulate death for testing
        //Invoke(nameof(Die), 5f); // Enemy will die after 5 seconds
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

    void Attack()
    {
        if (isAttacking) return; // If already attacking, exit to prevent multiple attacks

        isAttacking = true; // Start the attack sequence
        anim.SetBool("Walk", false); // Stop walking animation
        anim.SetTrigger("Attack"); // Trigger the attack animation
        rb.velocity = Vector2.zero; // Stop any movement during attack

        attackCount++;
        if (attackCount >= 4) // Flip every 4 attacks for variety
        {
            Flip(); // Flip direction after 4 attacks
            attackCount = 0;
        }

        StartCoroutine(ResetAttack()); // Reset attack flag after delay
    }

    IEnumerator ResetAttack()
    {
        yield return new WaitForSeconds(1f); // Attack cooldown
        isAttacking = false; // Allow next attack after cooldown
    }

    // Called at the attack animation event to deal damage to the player
    void DealDamage()
    {
        Collider2D hitPlayer = Physics2D.OverlapCircle(attackPoint.position, attackRange, LayerMask.GetMask("Player"));

        if (hitPlayer != null && !hitPlayer.GetComponent<PlayerControls>().IsDodging())
        {
            Debug.Log("Player detected! Attacking...");
            hitPlayer.GetComponent<PlayerBehaviour>().OnDeath(); // Player dies if hit by attack
        }
        else
        {
            Debug.Log("No player detected or player is dodging.");
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
        if (attackPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPoint.position, attackRange); // Visualize attack range
        }
    }

    // Behavior summary:
    // The BubbleEyeMutant patrols an area until it detects the player. Once detected, it switches to following the player.
    // It attacks the player at regular intervals and changes direction after a set number of attacks for variety.
    // If it hits an obstacle or reaches the end of its patrol area, it flips direction and resumes patrolling.
}
