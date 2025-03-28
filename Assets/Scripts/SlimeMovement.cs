using UnityEngine;

public class SlimeMovement : MonoBehaviour
{
    public float patrolSpeed = 1f;
    public Transform groundCheck;
    public Transform wallCheck;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private bool movingRight = true;
    private Animator animator;
    private float patrolTime = 0f;

    public int health = 20;
	public float hitCooldown = 0.5f;
	private float lastHitTime = 0f;

	void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        rb.freezeRotation = true;
    }

    void Update()
    {
        Patrol();

        if(health <= 0)
        {
            this.gameObject.SetActive(false);
        }
    }

    void Patrol()
    {
        animator.SetBool("Move", true);
        rb.velocity = new Vector2(movingRight ? patrolSpeed : -patrolSpeed, rb.velocity.y);

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

    bool IsGrounded()
    {
        return groundCheck && Physics2D.Raycast(groundCheck.position, Vector2.down, 0.2f, groundLayer);
    }

    bool IsHittingWall()
    {
        return Physics2D.Raycast(wallCheck.position, Vector2.right * (movingRight ? 1 : -1), 0.1f, groundLayer);
    }

    void Flip()
    {
        movingRight = !movingRight;
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }

	void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Player") && collision.gameObject.GetComponent<PlayerControls>().IsDodging() == false)
		{
			collision.gameObject.GetComponent<PlayerBehaviour>().PlayerHit();
		}
		else if (collision.CompareTag("Attack"))
		{
			if (Time.time - lastHitTime > hitCooldown)
			{
				health -= collision.gameObject.GetComponent<Attack>().getDamage();
				lastHitTime = Time.time;
			}
		}
	}
}
