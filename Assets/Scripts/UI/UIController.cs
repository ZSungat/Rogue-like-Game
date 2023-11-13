using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UIController : MonoBehaviour
{
    public static UIController instance;
    private void Awake()
    {
        instance = this;
    }
    public Slider ExpLevelSlider;
    public TMP_Text ExpLevelText;
    public GameObject LevelUpPanel;

    public LevelUpSelectionButton[] LevelUpButtons;

    public void UpdateExperience(int CurrentExp, int LevelExp, int CurrentLevel)
    {
        ExpLevelSlider.maxValue = LevelExp;
        ExpLevelSlider.value = CurrentExp;

        ExpLevelText.text = "Level: " + CurrentLevel;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //PauseUnpause();
        }
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
