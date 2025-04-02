using UnityEngine;

public class BossBehaviour : MonoBehaviour
{
    public float patrolSpeed = 1.5f;
    public float patrolCooldown = 2f;
    public float attackCooldown = 4f;
    public Transform launchOffset;
    public LayerMask groundLayer;
    public ProjectileBehaviour projectilePrefab;
    public int health = 14;
    public float hitCooldown = 0.5f;
    private float lastHitTime = 0f;

    private Rigidbody2D rb;
    private Animator animator;
    private bool movingRight = true;
    private float patrolTimer = 0f;
    private float attackTimer = 0f;
    private bool isAttacking = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        rb.freezeRotation = true;
    }

    void Update()
    {
        if (health <= 0)
        {
            Die();
            return;
        }

        // Track patrol and attack timing
        patrolTimer += Time.deltaTime;

        // If it's time to attack, stop patrolling and start the attack
        if (patrolTimer >= patrolCooldown && !isAttacking)
        {
            patrolTimer = 0f;
            Attack();
        }

        // If not attacking, continue patrolling
        if (!isAttacking)
        {
            Patrol();
        }

        // Handle attack logic
        if (isAttacking)
        {
            attackTimer += Time.deltaTime;
            if (attackTimer >= attackCooldown)
            {
                EndAttack();
            }
        }
    }

    void Patrol()
    {
        animator.SetBool("Walk", true);
        rb.velocity = new Vector2(movingRight ? patrolSpeed : -patrolSpeed, rb.velocity.y);
    }

    void Attack()
    {
        isAttacking = true;
        animator.SetBool("Walk", false);
        animator.SetBool("Attack", true);
        rb.velocity = Vector2.zero; // Stop movement during attack
        FireProjectile(); // Fire a projectile when attacking
    }

    // Called by animation event to fire the projectile
    public void FireProjectile()
    {
        if (projectilePrefab != null && launchOffset != null)
        {
            ProjectileBehaviour projectile = Instantiate(
                projectilePrefab,
                launchOffset.position,
                Quaternion.identity
            );
            projectile.SetDirection(movingRight ? Vector2.right : Vector2.left);
        }
        else
        {
            Debug.LogError("Projectile Prefab or Launch Offset not assigned!");
        }
    }

    void EndAttack()
    {
        isAttacking = false;
        animator.SetBool("Attack", false);
        attackTimer = 0f;
    }

    void Flip()
    {
        movingRight = !movingRight;
        transform.localScale = new Vector3(
            -transform.localScale.x,
            transform.localScale.y,
            transform.localScale.z
        );
    }

    // Function to handle damage from player attacks
    public void TakeDamage(int damage)
    {
        if (Time.time - lastHitTime > hitCooldown)
        {
            health -= damage;
            lastHitTime = Time.time;

            if (health <= 0)
            {
                Die();
            }
        }
    }

    void Die()
    {
        // Handle boss death (disable it for now)
        rb.velocity = Vector2.zero;
        rb.isKinematic = true; // Disable physics interaction after death
        GetComponent<Collider2D>().enabled = false; // Disable collisions after death
        Destroy(gameObject, 1.5f); // Destroy the enemy after a delay
    }
}
