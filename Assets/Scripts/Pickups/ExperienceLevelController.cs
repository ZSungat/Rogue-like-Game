using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperienceLevelController : MonoBehaviour
{
    public static ExperienceLevelController instance;
    public ExpPickup pickup;
    public List<int> ExpLevels;
    public int CurrentLevel = 1, LevelCount = 100;
    private void Awake()
    {
        instance = this;
    }

    public int CurrentExperience;
    void Start()
    {
        while (ExpLevels.Count < LevelCount)
        {
            ExpLevels.Add(Mathf.CeilToInt(ExpLevels[ExpLevels.Count - 1] * 1.1f));
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void GetExp(int AmountToGet)
    {
        CurrentExperience += AmountToGet;

        if (CurrentExperience >= ExpLevels[CurrentLevel])
        {
            LevelUp();
        }

        UIController.instance.UpdateExperience(CurrentExperience, ExpLevels[CurrentLevel], CurrentLevel);

        // SFXManager.instance.PlaySFXPitched(2);
    }
    public void SpawnExp(Vector3 position, int ExpValue)
    {
        Instantiate(pickup, position, Quaternion.identity).ExpValue = ExpValue;
    }

    private void LevelUp()
    {
        CurrentExperience -= ExpLevels[CurrentLevel];
        CurrentLevel++;
        if (CurrentLevel >= ExpLevels.Count)
        {
            CurrentLevel = ExpLevels.Count - 1;
        }

        //PlayerMovement.instance.ActiveWeapon.LevelUp();

        UIController.instance.LevelUpPanel.SetActive(true);

        Time.timeScale = 0f;

        //UIController.instance.LevelUpButtons[0].UpdateButtonDisplay(PlayerMovement.instance.ActiveWeapon);
    }
}
