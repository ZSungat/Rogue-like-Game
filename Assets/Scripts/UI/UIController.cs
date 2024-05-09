using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    public static UIController instance;
    private void Awake()
    {
        instance = this;
    }
    public Slider ExpLevelSlider;

    public GameObject LevelUpPanel;
    public GameObject PauseScreen;
    public GameObject LevelEndScreen;
    public LevelUpSelectionButton[] LevelUpButtons;
    public GameObject[] DescriptionButton;


    public TMP_Text EndTimeText;
    public TMP_Text ExpLevelText;
    public TMP_Text CoinText;
    public TMP_Text TimeText;

    public string MainMenuName;

    public PlayerStatUpgradeDisplay MoveSpeedUpgradeDisplay, HealthUpgradeDisplay, PickupRangeUpgradeDisplay, MaxWeaponsUpgradeDisplay;


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseUnpause();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            Restart();
        }




        // if (Input.GetKeyDown(Keyboard.LeftControl) && Input.GetKeyDown(KeyCode.Q))
        // {
        //     Restart();
        // }
        // if (Input.GetKeyDown(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.Alpha2))
        // {
        //     Restart();
        // }
        // if (Input.GetKeyDown(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.Alpha3))
        // {
        //     Restart();
        // }
        // if (Input.GetKeyDown(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.Alpha4))
        // {
        //     Restart();
        // }
    }
    public void UpdateExperience(int CurrentExp, int LevelExp, int CurrentLevel)
    {
        ExpLevelSlider.maxValue = LevelExp;
        ExpLevelSlider.value = CurrentExp;

        ExpLevelText.text = "Level  " + CurrentLevel;
    }
    public void SkipLevelUp()
    {
        LevelUpPanel.SetActive(false);
        Time.timeScale = 1f;
    }
    public void UpdateCoins()
    {
        CoinText.text = "Coins: " + CoinController.instance.CurrentCoins;
    }






    public void PurchaseMoveSpeed()
    {
        PlayerStatController.instance.PurchaseMoveSpeed();
        SkipLevelUp();
    }

    public void PurchaseHealth()
    {
        PlayerStatController.instance.PurchaseHealth();
        SkipLevelUp();
    }

    public void PurchasePickupRange()
    {
        PlayerStatController.instance.PurchasePickupRange();
        SkipLevelUp();
    }

    public void PurchaseMaxWeapons()
    {
        PlayerStatController.instance.PurchaseMaxWeapons();
        SkipLevelUp();
    }







    public void UpdateTimer(float time)
    {
        float minutes = Mathf.FloorToInt(time / 60f);
        float seconds = Mathf.FloorToInt(time % 60);

        TimeText.text = "Time: " + minutes + ":" + seconds.ToString("00");
    }
    public void QuitGame()
    {
        Application.Quit();
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1f;
    }
    public void GoToMainMenu()
    {
        SceneManager.LoadScene(MainMenuName);
        Time.timeScale = 1f;
    }
    public void PauseUnpause()
    {
        if (PauseScreen.activeSelf == false)
        {
            SFXManager.instance.StopMusic();
            PauseScreen.SetActive(true);
            Time.timeScale = 0f;
        }
        else
        {
            PauseScreen.SetActive(false);
            SFXManager.instance.PlayMusic(SFXManager.instance.currentIndex);

            if (LevelUpPanel.activeSelf == false)
            {
                Time.timeScale = 1f;
            }
        }
    }
}
