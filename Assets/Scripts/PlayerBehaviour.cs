using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerBehaviour : MonoBehaviour
{
    public GameObject deadCharacter;
    public GameObject Banner;

    private bool checkPoint;
    private Spawn spawn;
    private Vector2 checkPointPos;
    private int lives;

    public GameObject livesManager;

    private GameObject rightSlash;
    private GameObject leftSlash;

    private AudioSource audioSource;

    [SerializeField] private AudioClip deathSound;
    [SerializeField] private AudioClip deathSound2;
    [SerializeField] private AudioClip hurtSound;
    [SerializeField] private AudioClip hurtSound2;
    [SerializeField] private AudioClip pickupSound;

    private Attack attackR;
    private Attack attackL;
    public Image dbDmgImg;
    public Image jumpBoostImg;
    public Image speedBoostImg;

    private float defaultJumpPower = 15.0f;
    private float defaultRunSpeed = 8.0f;
    private float boostedJumpPower = 25.0f;  // Adjust as needed
    private float boostedRunSpeed = 12.0f;   // Adjust as needed
    private float powerupDuration = 10.0f;

    private Item nearbyItem;

    [Header("Health System")]
	public GameObject[] hitPoints; // UI GameObjects representing health
	private int currentHits = 0; // Tracks damage taken
	private bool canTakeDamage = true; // Controls damage delay
	public float hitDelay = 1.0f; // Delay time after each hit

	// Start is called before the first frame update
	void Start()
    {
        lives = 15;
        checkPoint = false;
        spawn = GameObject.FindGameObjectWithTag("Spawn").GetComponent<Spawn>();
        dbDmgImg.enabled = false;
        jumpBoostImg.enabled = false;
        speedBoostImg.enabled = false;

        rightSlash = GameObject.Find("Right Slash");
        leftSlash = GameObject.Find("Left Slash");

		attackR = rightSlash.GetComponent<Attack>();
		attackL = leftSlash.GetComponent<Attack>();

		ResetHitPoints();
    }

    // Update is called once per frame
    void Update()
    {
		if (Input.GetKeyDown(KeyCode.E))
		{
            OnDeath();
		}
        if (Input.GetKeyDown(KeyCode.F) && nearbyItem != null)
        {
            nearbyItem.storeItem(this); 
            nearbyItem = null; 
        }
    }

	/*
     * Decrements the lives veriable by 1
     * Checks if the lives varaible is less than 0
     * Calls the onDeath() method if the lives variable is less than 0
     */
	/*public void LoseLife()
    {
        lives--;//decrement lives variable
        if(lives < 0)//check lives variable against 0
        {
            OnDeath();//call onDeath method
        }
    }*/

	private void ChangeColor(GameObject obj, Color color)
	{
		obj.GetComponent<SpriteRenderer>().color = color;
	}

	public void PlayerHit()
	{
		if (canTakeDamage && currentHits < hitPoints.Length)
		{
			StartCoroutine(TakeDamageWithDelay());
		}
	}

	private IEnumerator TakeDamageWithDelay()
	{
        // play sound effect for taking damage
        if(Random.Range(0, 2) == 0)
            {
                SFXController.instance.PlaySoundFXClip(hurtSound, transform, 1f);
            }
            else
            {
                SFXController.instance.PlaySoundFXClip(hurtSound2, transform, 1f);
            }

		canTakeDamage = false; // Prevents taking damage multiple times in quick succession

		ChangeColor(hitPoints[currentHits], Color.red); // Change left-most hit point to red
		currentHits++;

		if (currentHits >= hitPoints.Length) // If all are red, lose a life
		{
			OnDeath();
		}

		yield return new WaitForSeconds(hitDelay); // Wait before allowing damage again
		canTakeDamage = true;
	}

	public void ResetHitPoints()
	{
		foreach (GameObject hitPoint in hitPoints)
		{
			ChangeColor(hitPoint, Color.green); // Reset all to green
		}
		currentHits = 0;
	}

	/*
     * Sets the values used for checking if the player has passed a checkpoint
     * and what the last passed checkpoint position is.
    */
	public void SetCheckpoint(Vector2 pos)
    {
        checkPoint = true;
        checkPointPos = pos;
    }

    /*
     * Method is used to respawn the player character at a vector2 position
     * Checks if the player has passed a checkpoint by calling the checkCheckPointed() method
     * if the player is checkpointed spawn the character at the postion of the checkpoint
     * if the player has not passed a checkpout spawn the character at the spawn point
     */
    public void RespawnPlayer()
    {
        if (checkPoint)
        {
            //Changes the position of the character to a new Vector2 position which uses the x and y position of the passed checkpoint
            gameObject.transform.position = new Vector2(checkPointPos.x, checkPointPos.y);
        }
        else
        {
            //Changes the position of the character to a new Vector2 position which uses the x and y values of the spawn point
            gameObject.transform.position = new Vector2(spawn.transform.position.x, spawn.transform.position.y);
        }
    }
	/*
     * Method is used for when the player character dies in the game.
     * It creates a dead body where the player died and respawns the player if they have sufficient lives.
     * But if the player does not have enough lives the player is taken back to the main menu
     */
	public void OnDeath()
	{
		if (lives > 0)
		{
            if(Random.Range(0, 2) == 0)
            {
                SFXController.instance.PlaySoundFXClip(deathSound, transform, 1f);
            }
            else
            {
                SFXController.instance.PlaySoundFXClip(deathSound2, transform, 1f);
            }
			Instantiate(deadCharacter, transform.position, Quaternion.identity);
			RespawnPlayer();
			lives--;

			Debug.Log("Player died, lives left: " + lives);

			LivesSystem livesSystem = livesManager.GetComponent<LivesSystem>();
			livesSystem.LoseLife();

			ResetHitPoints();
		}
		else
		{
			SceneManager.LoadScene(0);
		}
	}

	/*
     * this method is used to pause the player from performing any actions or moving
     * by using methods from the player control script and disabling the script in general
     * also displays a victory banner
     */
	public void PausePlayer()
    {
        gameObject.GetComponent<PlayerBehaviour>().DisplayBanner();//maybe should be moved to make pause more general
        //calls the SetVelocity() method from player controls and passes the method 0,0 to stop the player from moving
        gameObject.GetComponent<PlayerControls>().SetVelocity(0, 0);
        //calls the SetSpriteVolocity() method to change the sprite from running to idle animation
        gameObject.GetComponent<PlayerControls>().SetSpriteVolcity(0);
        //disables the PlayerControls script to stop getting inputs from the user
        gameObject.GetComponent<PlayerControls>().enabled = false;
    }
    public void PlayPlayer()
    {
        gameObject.GetComponent<PlayerControls>().enabled = true; 
    }
    public void DisplayBanner()
    {
        Instantiate(Banner, new Vector2(gameObject.transform.position.x, gameObject.transform.position.y), Quaternion.identity);
    }

    public void PickUp()
    {
        
    }

    public void AddLife(int life)
    {
        // play sound effect for picking up power up
        SFXController.instance.PlaySoundFXClip(pickupSound, transform, 1f);
        int previousLives = lives;

        if (lives + life <= 15)
        {
            lives += life;
        }
        else
        {
            lives = 15;
        }

        Debug.Log("Life updated: " + previousLives + " -> " + lives);

        LivesSystem livesSystem = livesManager.GetComponent<LivesSystem>();
        if (livesSystem != null)
        {
            livesSystem.UpdateLives(lives);
        }
        else
        {
            Debug.LogError("LivesSystem not found!");
        }
    }


    public void DoubleDmg()
    {
        // play sound effect for picking up power up
        SFXController.instance.PlaySoundFXClip(pickupSound, transform, 1f);
        Debug.Log("Double damage activated");
        attackR.damage *= 2;
        rightSlash.GetComponent<SpriteRenderer>().color = Color.magenta;
        attackL.damage *= 2;
        leftSlash.GetComponent<SpriteRenderer>().color = Color.magenta;
        dbDmgImg.enabled = true;
    }

    public void JumpBoost()
    {
        SFXController.instance.PlaySoundFXClip(pickupSound, transform, 1f);
        Debug.Log("Jump Boost Activated");
        jumpBoostImg.enabled = true;
        PlayerControls playerControls = GetComponent<PlayerControls>();
        if (playerControls != null)
        {
            playerControls.SetJumpPower(boostedJumpPower);
            StartCoroutine(ResetJumpBoost());
        }
    }

    private IEnumerator ResetJumpBoost()
    {
        yield return new WaitForSeconds(powerupDuration);
        PlayerControls playerControls = GetComponent<PlayerControls>();
        if (playerControls != null)
        {
            playerControls.SetJumpPower(defaultJumpPower);
            jumpBoostImg.enabled = false;
        }
    }

    public void SpeedBoost()
    {
        SFXController.instance.PlaySoundFXClip(pickupSound, transform, 1f);
        Debug.Log("Speed Boost Activated");
        speedBoostImg.enabled = true;
        PlayerControls playerControls = GetComponent<PlayerControls>();
        if (playerControls != null)
        {
            playerControls.SetRunSpeed(boostedRunSpeed);
            StartCoroutine(ResetSpeedBoost());
        }
    }

    private IEnumerator ResetSpeedBoost()
    {
        yield return new WaitForSeconds(powerupDuration);
        Debug.Log("Speed boost expired, resetting speed.");
        PlayerControls playerControls = GetComponent<PlayerControls>();
        if (playerControls != null)
        {
            playerControls.SetRunSpeed(defaultRunSpeed);
            speedBoostImg.enabled=false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Item"))
        {
            nearbyItem = collision.gameObject.GetComponent<Item>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Item"))
        {
            nearbyItem = null; 
        }
    }
}
