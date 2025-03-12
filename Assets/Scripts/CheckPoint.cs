using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    private bool passed;
    private Vector2 checkPointPos;
    private Animator animator;
    private PlayerBehaviour player;

    // Start is called before the first frame update
    void Start()
    {
        passed = false;
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerBehaviour>();
        checkPointPos = gameObject.transform.position;

    }

    // Update is called once per frame
    void Update()
    {
    
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && passed == false)
        {
            animator.SetBool("FlagRaise", true);
            passed = true;
            player.SetCheckpoint(checkPointPos);

        }
    }
}
