using System.Collections;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
	private float runSpeed = 5.0f;

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

	// Start is called before the first frame update
	void Start()
	{
		rigidBody = GetComponent<Rigidbody2D>();
		spriteRenderer = GetComponent<SpriteRenderer>();
		dodging = false;
	}

	// Update is called once per frame
	void Update()
	{
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

		NumberCheck();

		/*ColourCheck();*/
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
		dodgeTimer = Time.time + dodgeCooldown;

		float moveInput = Input.GetAxis("Horizontal");
		Vector2 dodgeDirection = GetDirection(moveInput);

		rigidBody.velocity = new Vector2(dodgeDirection.x * dodgeSpeed, 0);

		rigidBody.gravityScale = 0;

		yield return new WaitForSeconds(dodgeDuration);

		rigidBody.gravityScale = 1;
		dodging = false;
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

		attacking = true;
		attackTimer = Time.time + attackCooldown;

		float moveInput = Input.GetAxis("Horizontal");
		Vector2 attackDirection = GetDirection(moveInput);

		rigidBody.velocity = new Vector2(attackDirection.x * attackSpeed, 0);

		rigidBody.gravityScale = 0;

		yield return new WaitForSeconds(attackDuration);

		rigidBody.gravityScale = 1;
		attacking = false;

		if (attackCount == 3)
		{
			attackCount = 1;
		}
		else
		{
			attackCount++;
		}
		Debug.Log(attackCount);

		spriteRenderer.color = Color.white;
	}

	private Vector2 GetDirection(float moveInput)
	{
		Vector2 playerDirection = new Vector2(Mathf.Sign(moveInput), 0).normalized;
		return playerDirection;
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
