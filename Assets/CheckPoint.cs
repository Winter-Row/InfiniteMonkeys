using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    private bool passed;
    private SpriteRenderer sprite;
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            sprite.color = Color.green;
            passed = true;
        }
            
    }
}
