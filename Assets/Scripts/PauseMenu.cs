using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    private Canvas pauseMenu;

    void Start()
    {
        //get the canvas object from the scene
        pauseMenu = GetComponent<Canvas>();
        //disable the canvas object so the player cant see it
        pauseMenu.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        //check for the escape key being hit every frame
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //if the time scale is 1, so the game is running pause the game
            if(Time.timeScale == 1)
                PauseGame();
            else if(Time.timeScale == 0)//when the time scale is 0 so game is paused resume the game
                ResumeGame();
            
        }
    }
    /*
     * Used to pause the game and display the pause menu
     */
    public void PauseGame()
    {
        //enables the pause menu object to be visible to the player
        pauseMenu.enabled = true;
        //stops the games time from running pausing the game
        Time.timeScale = 0f;
    }
    /*
     * Used to resmue the game once the resume button is hit or the escape key is pressed again
     */
    public void ResumeGame()
    {
        //disables the pause menu so the player no longer sees the menu
        pauseMenu.enabled= false;
        //set the time scale to 1 so the game runs normally, resuming the game.
        Time.timeScale = 1f;
    }

}
