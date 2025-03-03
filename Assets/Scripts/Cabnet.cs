using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cabnet : MonoBehaviour
{
    private GameObject prompt;
    // Start is called before the first frame update
    void Start()
    {
        prompt = GameObject.Find("Prompt");
        prompt.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        prompt.SetActive(true);
        if (Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("Open");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        prompt.SetActive(false);
    }
}
