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
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerBehaviour>().displayBanner();
            collision.gameObject.GetComponent<PlayerControls>().SetVelocity(0, 0);
            collision.gameObject.GetComponent<PlayerControls>().SetSpriteVolcity(0);
            collision.gameObject.GetComponent<PlayerControls>().enabled = false;
        }
    }
}
