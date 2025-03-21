using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerBehaviour : MonoBehaviour
{
    public GameObject deadCharacter;
    public GameObject Banner;

    private bool checkPoint;
    private Spawn spawn;
    private Vector2 checkPointPos;
    private int lives;

    public GameObject livesManager;

    // Start is called before the first frame update
    void Start()
    {
        lives = 15;
        checkPoint = false;
        spawn = GameObject.FindGameObjectWithTag("Spawn").GetComponent<Spawn>();
    }

    // Update is called once per frame
    void Update()
    {
		if (Input.GetKeyDown(KeyCode.E))
		{
            OnDeath();
		}
	}

    /*
     * Decrements the lives veriable by 1
     * Checks if the lives varaible is less than 0
     * Calls the onDeath() method if the lives variable is less than 0
     */
    public void LoseLife()
    {
        lives--;//decrement lives variable
        if(lives < 0)//check lives variable against 0
        {
            OnDeath();//call onDeath method
        }
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
    public void respawnPlayer()
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
        if (lives > 0)//checks lives are greater than 0
        {
            //creates a deadCharacter prefab at the position of the current game object which is the player
            Instantiate(deadCharacter, new Vector2(gameObject.transform.position.x, gameObject.transform.position.y), Quaternion.identity);
            //calls the respawnPlayer() method
            respawnPlayer();
            //calls the LoseLife() method
            lives--;
            livesManager.GetComponent<LivesSystem>().LoseLife();
        }
        else
        {
            //loads the main menu sence which is set as sence 0 in the build settings
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
        gameObject.GetComponent<PlayerBehaviour>().displayBanner();//maybe should be moved to make pause more general
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
    public void displayBanner()
    {
        Instantiate(Banner, new Vector2(gameObject.transform.position.x, gameObject.transform.position.y), Quaternion.identity);
    }
}
