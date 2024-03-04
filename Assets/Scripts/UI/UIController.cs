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

    public TMP_Text ExpLevelText;
    public TMP_Text CoinText;
    public TMP_Text TimeText;

    public LevelUpSelectionButton[] LevelUpButtons;
    public GameObject[] DescriptionButton;


    public PlayerStatUpgradeDisplay MoveSpeedUpgradeDisplay, HealthUpgradeDisplay, PickupRangeUpgradeDisplay, MaxWeaponsUpgradeDisplay;


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseUnpause();
        }
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
    public void QuitGame()
    {
        Application.Quit();
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

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1f;
    }
    public void PauseUnpause()
    {
        if (PauseScreen.activeSelf == false)
        {
            PauseScreen.SetActive(true);
            Time.timeScale = 0f;
        }
        else
        {
            PauseScreen.SetActive(false);
            if (LevelUpPanel.activeSelf == false)
            {
                Time.timeScale = 1f;
            }
        }
    }
}
