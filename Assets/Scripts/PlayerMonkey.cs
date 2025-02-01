using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

public class PlayerMonkey : MonoBehaviour
{
    Rigidbody2D rigidbody;
    Animator animator;

    private float moveHorizontal;
    public float speed = 5f;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        moveHorizontal = Input.GetAxis("Horizontal");

        // Flip character sprite based on movement direction
        if (moveHorizontal > 0) // Moving right
        {
            GetComponent<SpriteRenderer>().flipX = false;
        }
        else if (moveHorizontal < 0) // Moving left
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }


        animator.SetFloat("xVelocity", Mathf.Abs(moveHorizontal));
        rigidbody.velocity = new Vector2(moveHorizontal * speed, rigidbody.velocity.y);

    }
}