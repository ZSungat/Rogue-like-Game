using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string FirstLevelName;


    public void StartGame()
    {
        
        SceneManager.LoadScene(FirstLevelName);
    }

    public void QuitGame()
    {
        Application.Quit();

        Debug.Log("I'm Quitting");
    }
}
