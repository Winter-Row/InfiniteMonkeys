using UnityEngine;

public class SlimeMovement : MonoBehaviour
{
    public float patrolSpeed = 1f; 
    public float chaseSpeed = 2f; 
    public float detectionRange = 6f;
    public float attackRange = 1.5f; 
    public float attackCooldown = 1f;
    public int health = 1;
    public Transform groundCheck; 
    public Transform wallCheck; 
    public LayerMask groundLayer;
    public LayerMask playerLayer;

    private Rigidbody2D rb;
    private Transform player;
    private bool movingRight = true; 
    private bool isChasing = false; 
    private float lastAttackTime = 0;
    private Animator animator; 
    private float moveDistance = 0f;
    private float patrolTime = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); 
        animator = GetComponent<Animator>(); 
        rb.freezeRotation = true; 
    }

    void Update()
    {
        DetectPlayer(); // Check if player is in range
        if (isChasing)
            ChasePlayer(); 
        else
            Patrol();
    }

    // Patrol the area by moving left and right
    void Patrol()
    {
        animator.SetBool("Move", true); 

        rb.velocity = new Vector2(movingRight ? patrolSpeed : -patrolSpeed, rb.velocity.y);

        moveDistance += Mathf.Abs(rb.velocity.x) * Time.deltaTime;
        patrolTime += Time.deltaTime;

        if (patrolTime >= 2f)
        {
            patrolTime = 0f;
            if (!IsGrounded() || IsHittingWall())
            {
                Flip();
            }
        }
    }

    // Check if the slime is grounded (on the ground)
    bool IsGrounded()
    {
        return groundCheck && Physics2D.Raycast(groundCheck.position, Vector2.down, 0.2f, groundLayer);
    }

    // Check if the slime is hitting a wall
    bool IsHittingWall()
    {
        return Physics2D.Raycast(wallCheck.position, Vector2.right * (movingRight ? 1 : -1), 0.1f, groundLayer);
    }

    // Detect if the player is in range
    void DetectPlayer()
    {
        Collider2D playerCollider = Physics2D.OverlapCircle(transform.position, detectionRange);
        if (playerCollider && playerCollider.CompareTag("Player")) // Check if the tag is "Player"
        {
            player = playerCollider.transform;
            isChasing = true;
        }
        else
        {
            isChasing = false;
        }
    }

    // Chase the player when in range
    void ChasePlayer()
    {
        animator.SetBool("Move", true); 

        float direction = player.position.x > transform.position.x ? 1 : -1;
        rb.velocity = new Vector2(direction * chaseSpeed, rb.velocity.y);

        if (Vector2.Distance(transform.position, player.position) <= attackRange)
        {
            Attack();
        }
    }

    // Perform the attack
    void Attack()
    {
        if (Time.time - lastAttackTime >= attackCooldown)
        {
            lastAttackTime = Time.time;
            animator.SetTrigger("Attack");
            player.GetComponent<PlayerBehaviour>().loseHealth(1);
        }
    }

    // Take damage when hit
    public void TakeDamage()
    {
        health--;
        if (health <= 0) 
        {
            animator.SetTrigger("Death"); 
            Destroy(gameObject, 0.5f); 
        }
    }

    // Flip the slime's direction when hitting a wall or ground
    void Flip()
    {
        movingRight = !movingRight;
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z); 
    }

    // Debug visualization for detection and attack ranges
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
