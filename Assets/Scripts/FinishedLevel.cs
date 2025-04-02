using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishedLevel : MonoBehaviour
{
    private int sceneNum;
    public GameObject Banner;
    private int slimesKilled = 0;
    private int requiredSlimes = 3;
    // Start is called before the first frame update
    void Start()
    {
        sceneNum = SceneManager.GetActiveScene().buildIndex;
    }
 

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SlimeKilled()
    {
        slimesKilled++;
        Debug.Log("Slimes killed: " + slimesKilled);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && slimesKilled >= requiredSlimes)
        {
            if (sceneNum == 1)
            {
                SceneManager.LoadScene(2);
            }
            else if (sceneNum == 2)
            {
                SceneManager.LoadScene(3);
            }
            else
            {
                collision.gameObject.GetComponent<PlayerBehaviour>().PausePlayer();
            }
        }
        else if (collision.gameObject.tag == "Player")
        {
            Debug.Log("You need to defeat more slimes before proceeding!");
        }
    }
}