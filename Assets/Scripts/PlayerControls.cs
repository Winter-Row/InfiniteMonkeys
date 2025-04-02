using System.Collections;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControls : MonoBehaviour
{
	private float runSpeed = 8.0f;
    private float moveHorizontal;
    private readonly float jumpPow = 15.0f;
	private readonly float maxFallSpeed = -30.0f;

	private readonly float dodgeSpeed = 5.0f;
	private readonly float dodgeDuration = 0.2f;
	private readonly float dodgeCooldown = 1.0f;
	private float dodgeTimer;
	private bool isDodging = false;

	private readonly float attackSpeed = 1.0f;
	private readonly float attackDuration = 0.2f;
	private readonly float attackCooldown = 1.0f;
	private float attackTimer;

	[SerializeField]
	private bool onGround;

	[SerializeField]
	private bool hasDoubleJump;

	private bool stomping;

	private bool dodging;
	private bool attacking;

	private Rigidbody2D rigidBody;

	private SpriteRenderer spriteRenderer;


    Animator animator;

    private GameObject rightSlash;
    private Animator rightSlashAnimator;

    private GameObject leftSlash;
	private Animator leftSlashAnimator;

	public GameObject stompBlast;

	private float playerDirection;

	public Collider2D groundCollider;

	public Collider2D triggerCollider;

	[SerializeField] private AudioClip jumpSound;
	[SerializeField] private AudioClip doubleJumpSound;

    // Start is called before the first frame update
    void Start()
	{
		rigidBody = GetComponent<Rigidbody2D>();
		spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        rightSlash = GameObject.Find("Right Slash");
        rightSlashAnimator = rightSlash.GetComponent<Animator>();
		leftSlash = GameObject.Find("Left Slash");
		leftSlashAnimator = leftSlash.GetComponent<Animator>();
		stompBlast = GameObject.Find("Stomp");


        
		rightSlash.gameObject.SetActive(false);
        leftSlash.gameObject.SetActive(false);
		stompBlast.gameObject.SetActive(false);
		attacking = false;
        dodging = false;
		stomping = false;
		rigidBody.gravityScale = 5;

		playerDirection = 1.0f;
    }

	// Update is called once per frame
	void Update()
	{
		SpeedControl();

		PlayerInputs();

		GravityControl();

		ChangeDirection();
    }

	void SpeedControl()
	{
		if (rigidBody.velocity.y < maxFallSpeed)
		{
			rigidBody.velocity = new Vector2(rigidBody.velocity.x, maxFallSpeed);
		}
	}

	void GravityControl()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			rigidBody.gravityScale = 1f;
		}
		else
		{
			rigidBody.gravityScale = 5f;
		}
	}

	void ChangeDirection()
	{
		if (GetComponent<SpriteRenderer>().flipX == true)
		{
			playerDirection = -1.0f;
		}

		else if (GetComponent<SpriteRenderer>().flipX == false)
		{
			playerDirection = 1.0f;
		}
	}

	void PlayerInputs()
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

		if (onGround && stomping)
		{
			StartCoroutine(StompBlast());
		}

		animator.SetFloat("xVelocity", Mathf.Abs(moveHorizontal));
		rigidBody.velocity = new Vector2(moveHorizontal * runSpeed, rigidBody.velocity.y);
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
			SFXController.instance.PlaySoundFXClip(jumpSound, transform, 1.0f);

			//Debug.Log(animator.GetBool("isJumping"));
		}
		else if (Input.GetKeyDown(KeyCode.Space) && !onGround && hasDoubleJump)
		{
			rigidBody.velocity = new Vector2(rigidBody.velocity.x, jumpPow);
			hasDoubleJump = false;
			SFXController.instance.PlaySoundFXClip(doubleJumpSound, transform, 1.0f);
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
		if (playerDirection == 1)
		{
			rightSlash.SetActive(true);
            rightSlashAnimator.SetBool("isAttackingRight", true);
            animator.SetBool("isAttacking", true);
        }
		else if(playerDirection == -1)
		{
			leftSlash.SetActive(true);
			leftSlashAnimator.SetBool("isAttackingLeft", true);
            animator.SetBool("isAttacking", true);
        }

		attacking = true;
		attackTimer = Time.time + attackCooldown;

		float moveInput = Input.GetAxisRaw("Horizontal");
		Vector2 attackDirection = GetDirection(playerDirection);

		rigidBody.gravityScale = 0;

		float elapsedTime = 0f;
		while (elapsedTime < attackDuration)
		{
			elapsedTime += Time.deltaTime;
			/*rigidBody.MovePosition(Vector2.Lerp(startPosition, targetPosition, elapsedTime / attackDuration));*/
			yield return null;
		}

		rigidBody.gravityScale = 2;

        attacking = false;
        animator.SetBool("isAttacking", false);

        // TW - waits to complete swing animation to reset
		if (rightSlash.activeSelf)
		{
            yield return new WaitUntil(() => !rightSlashAnimator.GetCurrentAnimatorStateInfo(0).IsName("BigSlashAnimation"));
            rightSlashAnimator.SetBool("isAttackingRight", false);
			rightSlash.SetActive(false);
		}
		else if (leftSlash.activeSelf)
		{
            yield return new WaitUntil(() => !leftSlashAnimator.GetCurrentAnimatorStateInfo(0).IsName("BigSlashAnimationLeft"));
            leftSlashAnimator.SetBool("isAttackingLeft", false);
            leftSlash.SetActive(false);
        }
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
		
	}

	void OnCollisionStay2D(Collision2D collision)
	{
		foreach (ContactPoint2D contact in collision.contacts)
		{
            if (contact.normal.y > 0.5f && !onGround)
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
		onGround = false;
	}
}
