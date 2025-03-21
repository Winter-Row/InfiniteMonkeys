using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishedLevel : MonoBehaviour
{
    private int sceneNum;
    public GameObject Banner;
    // Start is called before the first frame update
    void Start()
    {
        sceneNum = SceneManager.GetActiveScene().buildIndex;
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
            if(sceneNum == 1)
            {
                SceneManager.LoadScene(2);
            }
            else
            {
                //if the player tag is detected call the Pause Player Function
                collision.gameObject.GetComponent<PlayerBehaviour>().PausePlayer();
            }
            
        }
    }
}
