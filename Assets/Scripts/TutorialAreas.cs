using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialAreas : MonoBehaviour
{
    private ChangePrompt prompt;

    private CheckpointRoom checkpointRoom;
    // Start is called before the first frame update
    void Start()
    {
        prompt = GameObject.FindGameObjectWithTag("TextPrompt").GetComponent<ChangePrompt>();
        checkpointRoom = GameObject.FindGameObjectWithTag("CheckpointRoom").GetComponent<CheckpointRoom>();
        checkpointRoom.SetupRoom(0, 1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            prompt.changePrompt();
            gameObject.SetActive(false);
        }

    }
}
