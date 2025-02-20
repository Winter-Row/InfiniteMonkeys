using System.Collections;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
	private float runSpeed = 5.0f;
    private float moveHorizontal;
    private float jumpPow = 6.0f;

	private float dodgeSpeed = 15.0f;
	private float dodgeDuration = 0.2f;
	private float dodgeCooldown = 1.0f;
	private float dodgeTimer;

	private float attackSpeed = 10.0f;
	private float attackDuration = 0.1f;
	private float attackCooldown = 0.5f;
	private float attackTimer;

	private bool onGround;

	private bool hasDoubleJump;

	private bool dodging;
	private bool attacking;

	private int attackCount = 1;

	private Rigidbody2D rigidBody;

	private SpriteRenderer spriteRenderer;

    Animator animator;

    private GameObject rightSlash;
    private GameObject leftSlash;

    // Start is called before the first frame update
    void Start()
	{
		rigidBody = GetComponent<Rigidbody2D>();
		spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        rightSlash = GameObject.Find("Right Slash");
        leftSlash = GameObject.Find("Left Slash");
        rightSlash.gameObject.SetActive(false);
        leftSlash.gameObject.SetActive(false);
        attacking = false;
        dodging = false;
		rigidBody.gravityScale = 2;
	}

	// Update is called once per frame
	void Update()
	{
        moveHorizontal = Input.GetAxis("Horizontal");
        // Flip character sprite based on movement direction
        if (moveHorizontal > 0) // Moving right
        {
            GetComponent<SpriteRenderer>().flipX = false;
        }
        else if (moveHorizontal < 0) // Moving left
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }

        if (!dodging)
		{
			PlayerMove();
			Jump();
		}

		if (Input.GetKeyDown(KeyCode.B) && Time.time >= dodgeTimer)
		{
			StartCoroutine(Dodge());
		}

		if(Input.GetKeyDown(KeyCode.Q) && Time.time >= attackTimer)
		{
			StartCoroutine(Attack());
		}

		if(Input.GetKeyDown(KeyCode.DownArrow) && Time.time >= dodgeTimer)
		{
			Stomp();
		}

		/*NumberCheck();*/

        /*ColourCheck();*/
        animator.SetFloat("xVelocity", Mathf.Abs(moveHorizontal));
        rigidBody.velocity = new Vector2(moveHorizontal * runSpeed, rigidBody.velocity.y);
    }

	private void NumberCheck()
	{
		if (attackCount == 3)
		{
			attackSpeed = 20.0f;
		}
		else if (attackCount != 3)
		{
			attackSpeed = 10.0f;
		}
	}

	private void ColourCheck()
	{
		if (dodging)
		{
			spriteRenderer.color = Color.blue;
		}

		else if (!dodging)
		{
			spriteRenderer.color = Color.white;
		}

		if (attacking)
		{
			spriteRenderer.color = Color.red;
		}

		else if (attacking && attackCount == 3)
		{
			spriteRenderer.color = Color.cyan;
		}

		else if (!attacking)
		{
			spriteRenderer.color = Color.white;
		}
	}

	private void PlayerMove()
	{
        if (attacking)
        {
            runSpeed = 0.5f;
        }
        else if (!attacking)
        {
            runSpeed = 4.0f;
        }
        float playerInput = Input.GetAxis("Horizontal");
		rigidBody.velocity = new Vector2(playerInput * runSpeed, rigidBody.velocity.y);
	}

	private void Jump()
	{
		if (Input.GetKeyDown(KeyCode.Space) && onGround)
		{
			rigidBody.velocity = new Vector2(rigidBody.velocity.x, jumpPow);
		}
		else if (Input.GetKeyDown(KeyCode.Space) && !onGround && hasDoubleJump)
		{
			rigidBody.velocity = new Vector2(rigidBody.velocity.x, jumpPow);
			hasDoubleJump = false;
		}
	}

	private IEnumerator Dodge()
	{
		dodging = true;
        runSpeed = 0f;
        dodgeTimer = Time.time + dodgeCooldown;

		float moveInput = Input.GetAxis("Horizontal");
		Vector2 dodgeDirection = GetDirection(moveInput);

		rigidBody.velocity = new Vector2(dodgeDirection.x * dodgeSpeed, 0);

		rigidBody.gravityScale = 0;

		yield return new WaitForSeconds(dodgeDuration);

        rigidBody.gravityScale = 2;
        dodging = false;
        runSpeed = 4.0f;
    }

	private IEnumerator Attack()
	{
        if (attackCount < 3)
		{
			spriteRenderer.color = Color.red;
			attackSpeed = 0;
		}

		else if (attackCount == 3)
		{
			spriteRenderer.color = Color.cyan;
			attackSpeed = 30.0f;
		}

        if (GetComponent<SpriteRenderer>().flipX == false)
        {
            rightSlash.SetActive(true);
        }
        else if (GetComponent<SpriteRenderer>().flipX)
        {
            leftSlash.SetActive(true);
        }

        attacking = true;
		attackTimer = Time.time + attackCooldown;

		float moveInput = Input.GetAxis("Horizontal");
		Vector2 attackDirection = GetDirection(moveInput);

		rigidBody.velocity = new Vector2(attackDirection.x * attackSpeed, 0);

		rigidBody.gravityScale = 0;

		yield return new WaitForSeconds(attackDuration);

		rigidBody.gravityScale = 2;
		attacking = false;

        rightSlash.SetActive(false);
        leftSlash.SetActive(false);

        if (attackCount == 3)
		{
			attackCount = 1;
		}
		else
		{
			attackCount++;
		}

		spriteRenderer.color = Color.white;
	}

    private Vector2 GetDirection(float moveInput)
    {
        Vector2 playerDirection = new Vector2(Mathf.Sign(moveInput), 0).normalized;
        return playerDirection;
    }

	private void Stomp()
	{
		if (!onGround)
		{
			rigidBody.velocity = new Vector2(rigidBody.velocity.x, -50.0f);
		}
	}

    void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.contacts[0].normal.y > 0.5f)
		{
			onGround = true;
			hasDoubleJump = true;
		}
	}

	void OnCollisionExit2D(Collision2D collision)
	{
		onGround = false;
	}
}
