using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SlimUI.ModernMenu;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public MainMenuNew cameraScript;

    bool isAnyKeyPressed;

    // Start is called before the first frame update
    void Start()
    {
        isAnyKeyPressed = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(!isAnyKeyPressed && Input.anyKeyDown)
        {
            GotoMainWindow();
            isAnyKeyPressed = true;
        }
    }

    void GotoMainWindow()
    {
        cameraScript.Position2();
        cameraScript.PlaySwoosh();
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
