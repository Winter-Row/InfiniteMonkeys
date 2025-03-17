using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    private GameObject optionsMenu;
    private GameObject menuBtns;
    private int level;
    // Start is called before the first frame update
    void Start()
    {
        level = 1;
        optionsMenu = GameObject.FindGameObjectWithTag("Options");
        optionsMenu.SetActive(false);
        menuBtns = GameObject.FindGameObjectWithTag("MenuBtns");
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
        menuBtns.SetActive(false);
        optionsMenu.SetActive(true);
        //Instantiate(optionsMenu, new Vector2(gameObject.transform.position.x, gameObject.transform.position.y), Quaternion.identity);
        Debug.Log("Options is W.I.P");
    }

    public void Back()
    {
        menuBtns.SetActive(true);
        if (optionsMenu.activeSelf)
        {
            optionsMenu.SetActive(false);
        }
    }

    public void toggleFullScreen(Toggle toggle)
    {
        if (toggle.isOn)
        {
            Screen.fullScreen = true;
        }
        else
        {
            Screen.fullScreen = false;
        }
        
        Debug.Log(Screen.fullScreen);
    }

    public void exitGame()
    {
        Application.Quit();
    }
}
