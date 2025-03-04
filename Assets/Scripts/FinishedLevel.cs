using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishedLevel : MonoBehaviour
{
    public GameObject Banner;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //checks the collided game object to see if it has the player tag
        if (collision.gameObject.tag == "Player")
        {
            //if the player tag is detected call the Pause Player Function
            collision.gameObject.GetComponent<PlayerBehaviour>().PausePlayer();
        }
    }
}
