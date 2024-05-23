using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string FirstLevelName;
    public GameObject CreditCanvas;
    public GameObject SettingsCanvas;


    public void StartGame()
    {

        SceneManager.LoadScene(FirstLevelName);
    }

    public void QuitGame()
    {
        Application.Quit();

        Debug.Log("I'm Quitting");
    }

    public void CreditScreen()
    {
        if (!CreditCanvas.activeSelf)
        {
            CreditCanvas.SetActive(true);
        }
        else
        {
            CreditCanvas.SetActive(false);
        }

    }
    public void SettingsScreen()
    {
        if (!SettingsCanvas.activeSelf)
        {
            SettingsCanvas.SetActive(true);
        }
        else
        {
            SettingsCanvas.SetActive(false);
        }

    }

    public void GoToGithub()
    {
        Application.OpenURL("https://github.com/ZSungat/");
    }
}
