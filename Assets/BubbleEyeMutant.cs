using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleEyeMutant : MonoBehaviour
{
    public float moveSpeed = 2f;  // How fast the enemy moves
    public float detectionRange = 3f;  // Range for detecting the player
    public Transform player;  // Player reference
    public Transform groundCheck, wallCheck;  // Ground & wall checks
    public LayerMask groundLayer;  // Layer to check for ground
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
    private float maxPatrolDistance = 3f;  // How far he walks before flipping

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        rb.freezeRotation = true;
    }

    void Update()
    {
        // Trying to get player detection working but still kinda broken
        float playerDistance = Vector2.Distance(transform.position, player.position);

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

        if (isFollowing && !isAttacking)
        {
            FollowPlayer();
        }
        else if (isPatrolling && !isAttacking)
        {
            Patrol();
        }
    }

    void Patrol()
    {
        if (Time.time < patrolCooldown) return;  // Small delay before moving again

        anim.SetBool("Walk", true);
        rb.velocity = new Vector2(movingRight ? moveSpeed : -moveSpeed, rb.velocity.y);
        patrolDistance += moveSpeed * Time.deltaTime;

        // Trying to make him walk further before flipping
        if (patrolDistance >= maxPatrolDistance || !IsGrounded() || IsHittingWall())
        {
            Flip();
            patrolDistance = 0;
            patrolCooldown = Time.time + 1f;  // Wait a sec before flipping again
        }
    }

    void FollowPlayer()
    {
        anim.SetBool("Walk", true);
        float direction = (player.position.x > transform.position.x) ? 1 : -1;

        rb.velocity = new Vector2(direction * moveSpeed, rb.velocity.y);

        // Trying to make sure he turns to face player but feels off
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
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }

    void Attack()
    {
        if (isAttacking) return;  // Avoid spamming attacks

        isAttacking = true;
        anim.SetBool("Walk", false);
        anim.SetTrigger("Attack");
        rb.velocity = Vector2.zero;

        StartCoroutine(ResetAttack());
    }

    IEnumerator ResetAttack()
    {
        yield return new WaitForSeconds(1f);  // Gives time for attack animation
        isAttacking = false;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // Not sure why he's not attacking, need to debug
        if (collision.CompareTag("Player") && !isAttacking)
        {
            Attack();
            collision.gameObject.GetComponent<PlayerBehaviour>().onDeath();
        }
    }

    public void TakeDamage()
    {
        health--;
        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        anim.SetTrigger("Death");
        rb.velocity = Vector2.zero;
        rb.isKinematic = true;
        GetComponent<Collider2D>().enabled = false;
        Destroy(gameObject, 1.5f);
    }
}
