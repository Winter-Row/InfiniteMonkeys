using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialAreas : MonoBehaviour
{
    private ChangePrompt prompt;
    // Start is called before the first frame update
    void Start()
    {
        prompt = GameObject.FindGameObjectWithTag("TextPrompt").GetComponent<ChangePrompt>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        prompt.changePrompt();
        gameObject.SetActive(false);
    }
}
