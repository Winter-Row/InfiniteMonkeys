using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    private int level;
    // Start is called before the first frame update
    void Start()
    {
        level = 1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void loadLevel()
    {
        SceneManager.LoadScene(level);
    }

    public void LoadMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    public void LoadOptions()
    {
        Debug.Log("Options is W.I.P");
    }

    public void exitGame()
    {
        Application.Quit();
    }
}
