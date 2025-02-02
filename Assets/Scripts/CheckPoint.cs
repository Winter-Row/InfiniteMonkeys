using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    private bool passed;
    private SpriteRenderer sprite;
    private Vector2 checkPointPos;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        passed = false;
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        checkPointPos = gameObject.transform.position;

    }

    // Update is called once per frame
    void Update()
    {
    
    }

    public Vector2 getCheckPointPos()
    {
        return checkPointPos;
    }

    public bool checkCheckPointed()
    {
        return passed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && passed == false)
        {
            animator.SetBool("FlagRaise", true);
            passed = true;
        }
    }
}
