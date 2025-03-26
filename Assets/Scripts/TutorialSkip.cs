using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialSkip : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void skipTutorial()
    {
        SceneManager.LoadScene(2);
    }
    public void playTutorial()
    {
        gameObject.SetActive(false);
    }
}
