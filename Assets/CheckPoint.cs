using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    private bool passed;
    private SpriteRenderer sprite;
    private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        passed = false;
        sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
    
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && passed == false)
        {
            sprite.color = Color.green;
            passed = true;
        }
    }



    private void RespawnPlayer()
    {

    }
}
