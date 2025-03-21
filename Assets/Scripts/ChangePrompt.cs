using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ChangePrompt : MonoBehaviour
{
    private Text prompt;
    private int promptNum;
    // Start is called before the first frame update
    void Start()
    {
        prompt =  gameObject.GetComponent<Text>();
        promptNum = 1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void changePrompt()
    {
        promptNum++;
        Debug.Log(promptNum);
        switch (promptNum)
        {
            case 2:
                prompt.text = "I can use the space bar to jump this gap";
                break;
            case 3:
                prompt.text = "Ah nice a checkpoint I can spawn back here if i die";
                break;
            case 4:
                prompt.text = "O no an enemy I need to use Q to hit it with my weapon";
                break;
            case 5:
                prompt.text = "I should go see whats down there";
                break;
            case 6:
                prompt.text = "O i bet there is an item in there I can get it by breaking it with F";
                break;
            case 7:
                prompt.text = "All I need to know for now lets head to the main lab";
                break;
        }
    }
}
