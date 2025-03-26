using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControls : MonoBehaviour
{
	private float runSpeed = 8.0f;
    private float moveHorizontal;
    private float jumpPow = 15.0f;

	private float dodgeSpeed = 5.0f;
	private float dodgeDuration = 0.2f;
	private float dodgeCooldown = 1.0f;
	private float dodgeTimer;
	private bool isDodging = false;

	private float attackSpeed = 1.0f;
	private float attackDuration = 0.1f;
	private float attackCooldown = 0.5f;
	private float attackTimer;

	[SerializeField]
	private bool onGround;

	[SerializeField]
	private bool hasDoubleJump;

	private bool stomping;

	private bool canClimb;

	private bool dodging;
	private bool attacking;

	private bool climbing;

	private int attackCount = 1;

	private Rigidbody2D rigidBody;

	private SpriteRenderer spriteRenderer;

    Animator animator;

    private GameObject rightSlash;
    private GameObject leftSlash;

	public GameObject stompBlast;

	private Collider2D currentPlatform;
	public LayerMask passThroughMask;

	private float playerDirection;

    // Start is called before the first frame update
    void Start()
	{
		rigidBody = GetComponent<Rigidbody2D>();
		spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        rightSlash = GameObject.Find("Right Slash");
        leftSlash = GameObject.Find("Left Slash");
		stompBlast = GameObject.Find("Stomp");
        rightSlash.gameObject.SetActive(false);
        leftSlash.gameObject.SetActive(false);
		stompBlast.gameObject.SetActive(false);
        attacking = false;
        dodging = false;
		stomping = false;
		canClimb = false;

		climbing = false;
		rigidBody.gravityScale = 5;

		playerDirection = 1.0f;
    }

	// Update is called once per frame
	void Update()
	{
		float maxFallSpeed = -30f;

		if (rigidBody.velocity.y < maxFallSpeed)
		{
			rigidBody.velocity = new Vector2(rigidBody.velocity.x, maxFallSpeed);
		}

		IsClimbing();
		

		if(canClimb && Input.GetKeyDown(KeyCode.UpArrow))
		{
			climbing = true;
			Climb();
		}

		if(climbing && Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow))
		{
			climbing = false;
		}

		if (Input.GetKeyDown(KeyCode.Space))
		{
			rigidBody.gravityScale = 1f;
		}
		else
		{
			rigidBody.gravityScale = 5f;
		}

		if(GetComponent<SpriteRenderer>().flipX == true)
		{
			playerDirection = -1.0f;
		}

		else if(GetComponent<SpriteRenderer>().flipX == false)
		{
			playerDirection = 1.0f;
		}
    }

	private void IsClimbing()
	{
        if (!climbing)
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

			if (Input.GetKeyDown(KeyCode.Q) && Time.time >= attackTimer)
			{
				StartCoroutine(Attack());
			}

			if (Input.GetKeyDown(KeyCode.DownArrow) && !Input.GetKeyDown(KeyCode.Space))
			{
				Stomp();
			}

			if (Input.GetKey(KeyCode.DownArrow) && onGround && Input.GetKeyDown(KeyCode.Space))
			{
				DropDown();
			}

			if (onGround && stomping)
			{
				StartCoroutine(StompBlast());
			}

			animator.SetFloat("xVelocity", Mathf.Abs(moveHorizontal));
			rigidBody.velocity = new Vector2(moveHorizontal * runSpeed, rigidBody.velocity.y);
		}
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
            runSpeed = 8.0f;
        }
        float playerInput = Input.GetAxis("Horizontal");
		rigidBody.velocity = new Vector2(playerInput * runSpeed, rigidBody.velocity.y);
	}

	private void Jump()
	{
		if (Input.GetKeyDown(KeyCode.Space) && onGround && !Input.GetKey(KeyCode.DownArrow))
		{
            animator.SetBool("isJumping", true);
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, jumpPow);
            
            //Debug.Log(animator.GetBool("isJumping"));
        }
		else if (Input.GetKeyDown(KeyCode.Space) && !onGround && hasDoubleJump)
		{
			rigidBody.velocity = new Vector2(rigidBody.velocity.x, jumpPow);
			hasDoubleJump = false;
			animator.SetBool("isJumping", false);
			animator.SetBool("isDoubleJumping", true);
		}
	}

	private IEnumerator Dodge()
	{
		dodging = true;
		runSpeed = 0f;
		dodgeTimer = Time.time + dodgeCooldown;

		// Determine dodge direction based on playerDirection
		Vector2 dodgeDirection = new Vector2(playerDirection, 0).normalized;
		Vector2 startPosition = rigidBody.position;
		Vector2 targetPosition = startPosition + dodgeDirection * dodgeSpeed;

		// Temporarily disable gravity
		rigidBody.gravityScale = 0;

		float elapsedTime = 0f;
		while (elapsedTime < dodgeDuration)
		{
			elapsedTime += Time.deltaTime;
			rigidBody.MovePosition(Vector2.Lerp(startPosition, targetPosition, elapsedTime / dodgeDuration));
			yield return null;
		}

		// Restore gravity and normal movement
		rigidBody.gravityScale = 5;
		dodging = false;
		runSpeed = 8.0f;
	}

	private IEnumerator Attack()
	{
		/*if (attackCount < 3)
		{
			spriteRenderer.color = Color.red;
			attackSpeed = 1.0f;
		}
		else if (attackCount == 3)
		{
			spriteRenderer.color = Color.cyan;
			attackSpeed = 2.0f;
		}*/

		if (playerDirection == 1)
		{
			rightSlash.SetActive(true);
            animator.SetBool("isAttacking", true);
        }
		else if(playerDirection == -1)
		{
			leftSlash.SetActive(true);
            animator.SetBool("isAttacking", true);
        }

		attacking = true;
		attackTimer = Time.time + attackCooldown;

		float moveInput = Input.GetAxisRaw("Horizontal");
		Vector2 attackDirection = GetDirection(playerDirection);
		Vector2 startPosition = rigidBody.position;
		Vector2 targetPosition = startPosition + attackDirection * attackSpeed;

		rigidBody.gravityScale = 0;

		float elapsedTime = 0f;
		while (elapsedTime < attackDuration)
		{
			elapsedTime += Time.deltaTime;
			rigidBody.MovePosition(Vector2.Lerp(startPosition, targetPosition, elapsedTime / attackDuration));
			yield return null;
		}

		rigidBody.gravityScale = 2;
		attacking = false;
		animator.SetBool("isAttacking", false);
		rightSlash.SetActive(false);
		leftSlash.SetActive(false);

		/*if (attackCount == 3)
		{
			attackCount = 1;
		}
		else
		{
			attackCount++;
		}

		spriteRenderer.color = Color.white;*/
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
			stomping = true;
			rigidBody.velocity = new Vector2(rigidBody.velocity.x, -50.0f);
		}
	}

	private IEnumerator StompBlast()
	{
		stompBlast.SetActive(true);

		yield return new WaitForSeconds(0.2f);

		stompBlast.SetActive(false);
		stomping = false;
	}

	private void DropDown()
	{
		if (currentPlatform != null)
		{
			StartCoroutine(PassThrough());
		}
		Debug.Log("Pass through");
	}

	IEnumerator PassThrough()
	{
		if (currentPlatform != null)
		{
			Collider2D platformCollider = currentPlatform;
			platformCollider.enabled = false;
			yield return new WaitForSeconds(0.5f);
			platformCollider.enabled = true;
		}
	}

	private void Climb()
	{
		rigidBody.velocity = Vector2.zero;
		rigidBody.gravityScale = 0;
		rigidBody.constraints = RigidbodyConstraints2D.FreezePositionX;
		rigidBody.constraints = RigidbodyConstraints2D.FreezeRotation;

		if(Input.GetKey(KeyCode.UpArrow))
		{
			rigidBody.velocity = new Vector2(rigidBody.velocity.x, 2.0f);
		}

		else if (Input.GetKey(KeyCode.DownArrow))
		{
			rigidBody.velocity = new Vector2(rigidBody.velocity.x, -2.0f);
		}

		else if (!Input.GetKey(KeyCode.DownArrow) || !Input.GetKey(KeyCode.UpArrow))
		{
			rigidBody.velocity = Vector2.zero;
		}
	}

	//public method to set the run speed with a provided value speed
	public void SetVelocity(float speedx, float speedy)
	{
		rigidBody.velocity = new Vector2(speedx,speedy);
	}
	//public method for chaning the xVelocity sprite condition
	public void SetSpriteVolcity(float volocity)
	{
		animator.SetFloat("xVelocity", volocity);
    }
    
	public bool IsDodging()
	{
		return isDodging;
	}

    void OnCollisionEnter2D(Collision2D collision)
	{
		if (((1 << collision.gameObject.layer) & passThroughMask) != 0)
		{
			currentPlatform = collision.collider;
		}

		if (collision.gameObject.layer == LayerMask.NameToLayer("Climbable"))
		{
			Debug.Log("Can climb");
			canClimb = true;
		}
	}

	void OnCollisionStay2D(Collision2D collision)
	{
		foreach (ContactPoint2D contact in collision.contacts)
		{
			if (contact.normal.y > 0.5f)
			{
				onGround = true;
				animator.SetBool("isDoubleJumping", false);
				animator.SetBool("isJumping", false);
				hasDoubleJump = true;
				return;
			}
		}
	}

	void OnCollisionExit2D(Collision2D collision)
	{
		onGround = false; // Player left the ground
	}

	/*void OnCollisionExit2D(Collision2D collision)
	{
		*//*onGround = false;*//*

		if (collision.collider == currentPlatform)
		{
			currentPlatform = null;
		}

		if (collision.gameObject.layer == LayerMask.NameToLayer("Climbable"))
		{
			Debug.Log("Can't climb");
			canClimb = false;
			rigidBody.gravityScale = 2;
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.layer == LayerMask.NameToLayer("Climbable"))
		{
			Debug.Log("Can climb");
			canClimb = true;
		}
	}

    private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.gameObject.layer == LayerMask.NameToLayer("Climbable"))
		{
			Debug.Log("Can't climb");
			canClimb = false;
			climbing = false;
			rigidBody.gravityScale = 2;
		}
	}*/
}
