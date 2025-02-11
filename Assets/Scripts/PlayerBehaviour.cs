using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    public GameObject deadCharacter;

    private CheckPoint checkPoint;
    private Vector2 checkPointPos;
    private int health;
    // Start is called before the first frame update
    void Start()
    {
        checkPoint = GameObject.FindGameObjectWithTag("CheckPoint").GetComponent<CheckPoint>();
        health = 100;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void loseHealth(int dmg)
    {
        health -= dmg;
        if(health < 0)
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
    }

    public void onDeath()
    {
        Instantiate(deadCharacter, new Vector2(gameObject.transform.position.x, gameObject.transform.position.y), Quaternion.identity);
        respawnPlayer();
        
    }
}
