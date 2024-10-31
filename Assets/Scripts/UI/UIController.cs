using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class UIController : MonoBehaviour
{
    #region Singleton
    public static UIController instance;
    private void Awake()
    {
        instance = this;
    }
    #endregion

    #region UI References
    [Header("UI Panels")]
    public GameObject LevelUpPanel;
    public GameObject PauseScreen;
    public GameObject LevelEndScreen;
    public GameObject SettingsScreen;

    [Header("Level Up UI")]
    public LevelUpSelectionButton[] LevelUpButtons;
    public GameObject[] DescriptionButton;

    [Header("UI Text Elements")]
    public TMP_Text EndTimeText;
    public TMP_Text ExpLevelText;
    public TMP_Text CoinText;
    public TMP_Text TimeText;
    public TMP_Text FpsCounterText;

    [Header("Progress Bars")]
    public Slider ExpLevelSlider;

    [Header("Upgrade Displays")]
    public PlayerStatUpgradeDisplay MoveSpeedUpgradeDisplay;
    public PlayerStatUpgradeDisplay HealthUpgradeDisplay;
    public PlayerStatUpgradeDisplay PickupRangeUpgradeDisplay;
    public PlayerStatUpgradeDisplay MaxWeaponsUpgradeDisplay;
    #endregion

    #region Private Variables
    private float currentFps;
    private float smoothedFps;
    private float smoothingFactor = 0.4f; // Adjust this to control how much weight is given to recent FPS
    public string MainMenuName;
    #endregion

    #region Unity Lifecycle Methods
    private IEnumerator Start()
    {
        while (true)
        {
            currentFps = 1f / Time.unscaledDeltaTime;
            smoothedFps = (smoothingFactor * currentFps) + (1f - smoothingFactor) * smoothedFps;
            yield return new WaitForSeconds(0.1f);
        }
    }

    void Update()
    {
        HandleInput();
        UpdateFPSCounter();
    }
    #endregion

    #region Input Handling
    private void HandleInput()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (SettingsScreen.activeSelf)
            {
                Settings();
            }
            else
            {
                PauseUnpause();
            }
        }
    }
    #endregion

    #region UI Updates
    public void UpdateExperience(int CurrentExp, int LevelExp, int CurrentLevel)
    {
        ExpLevelSlider.maxValue = LevelExp;
        ExpLevelSlider.value = CurrentExp;
        ExpLevelText.text = "Level  " + CurrentLevel;
    }

    public void UpdateCoins()
    {
        CoinText.text = "Coins: " + CoinController.instance.CurrentCoins;
    }

    public void UpdateTimer(float time)
    {
        float minutes = Mathf.FloorToInt(time / 60f);
        float seconds = Mathf.FloorToInt(time % 60);
        TimeText.text = "Time: " + minutes + ":" + seconds.ToString("00");
    }

    private void UpdateFPSCounter()
    {
        FpsCounterText.text = "" + Mathf.Round(smoothedFps);
    }
    #endregion

    #region Player Stat Purchases
    public void PurchaseMoveSpeed()
    {
        PlayerStatController.instance.PurchaseMoveSpeed();
    }

    public void PurchaseHealth()
    {
        PlayerStatController.instance.PurchaseHealth();
    }

    public void PurchasePickupRange()
    {
        PlayerStatController.instance.PurchasePickupRange();
    }

    public void PurchaseMaxWeapons()
    {
        PlayerStatController.instance.PurchaseMaxWeapons();
    }
    #endregion

    #region Game State Management
    public void SkipLevelUp()
    {
        LevelUpPanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void PauseUnpause()
    {
        if (PauseScreen.activeSelf == false)
        {
            PauseGame();
        }
        else
        {
            UnpauseGame();
        }
    }

    private void PauseGame()
    {
        if (SFXManager.instance != null)
        {
            SFXManager.instance.StopMusic();
        }
        PauseScreen.SetActive(true);
        Time.timeScale = 0f;
    }

    private void UnpauseGame()
    {
        PauseScreen.SetActive(false);
        if (SFXManager.instance != null)
        {
            SFXManager.instance.PlayMusic(SFXManager.instance.currentIndex);
        }

        if (LevelUpPanel.activeSelf == false)
        {
            Time.timeScale = 1f;
        }
    }
    #endregion

    #region Scene Management
    public void Settings()
    {
        if (SettingsScreen.activeSelf == false)
        {
            SFXManager.instance.StopMusic();
            SettingsScreen.SetActive(true);
            PauseScreen.SetActive(false);
            Time.timeScale = 0f;
        }
        else
        {
            SettingsScreen.SetActive(false);
            PauseUnpause();
        }
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

    public void QuitGame()
    {
        Application.Quit();
    }
    #endregion
}