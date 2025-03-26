using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cabinet : MonoBehaviour
{
    private GameObject prompt;
    private Animator animator;
    private List<GameObject> itemList;
    private bool opened;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        prompt = GameObject.Find("Prompt");
        prompt.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setOpened(bool flag)
    {
        opened = flag;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(opened == false)
        {
            prompt.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        prompt.SetActive(false);
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (Input.GetKeyDown(KeyCode.F) && !opened)
            {
                animator.SetBool("Break", true);
                prompt.SetActive(false);
                setOpened(true);
                Debug.Log("Open");
            }
        }
    }
}
