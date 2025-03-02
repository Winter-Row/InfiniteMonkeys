using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerBehaviour : MonoBehaviour
{
    public GameObject deadCharacter;
    public GameObject Banner;

    private CheckPoint checkPoint;
    private Spawn spawn;
    private Vector2 checkPointPos;
    private int lives;
    // Start is called before the first frame update
    void Start()
    {
        checkPoint = GameObject.FindGameObjectWithTag("CheckPoint").GetComponent<CheckPoint>();
        spawn = GameObject.FindGameObjectWithTag("Spawn").GetComponent<Spawn>();
        lives = 15;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void loseHealth(int dmg)
    {
        lives -= dmg;
        if(lives < 0)
        {
            onDeath();
        }
    }

    public void setCheckPointPos(Vector2 pos)
    {
        checkPointPos = pos;
    }

    public void respawnPlayer()
    {
        if (checkPoint.checkCheckPointed())
        {
            gameObject.transform.position = new Vector2(checkPointPos.x, checkPointPos.y);
        }
        else
        {
            //for when lives are implemented
            gameObject.transform.position = new Vector2(spawn.transform.position.x, spawn.transform.position.y);
            //for now when player dies take back to the menu
            /*SceneManager.LoadScene(0);*/
        }
    }

    public void onDeath()
    {
        if (lives > 0)
        {
            Instantiate(deadCharacter, new Vector2(gameObject.transform.position.x, gameObject.transform.position.y), Quaternion.identity);
            respawnPlayer();
            lives -= 1;
            Debug.Log(lives);
        }
        else
        {
            SceneManager.LoadScene(0);
        }

	}
    
    public void displayBanner()
    {
        Instantiate(Banner, new Vector2(gameObject.transform.position.x, gameObject.transform.position.y), Quaternion.identity);
    }
}
