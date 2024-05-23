using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperienceLevelController : MonoBehaviour
{
    public static ExperienceLevelController instance;
    public ExpPickup pickup;
    public List<int> ExpLevels;
    public List<Weapon> WeaponsToUpgrade;
    public int CurrentLevel = 1, LevelCount = 100;
    private void Awake()
    {
        instance = this;
    }

    public int CurrentExperience;
    // Start is called before the first frame update
    void Start()
    {
        while (ExpLevels.Count < LevelCount)
        {
            ExpLevels.Add(Mathf.CeilToInt(ExpLevels[ExpLevels.Count - 1] * 1.1f));
        }
    }

    public void GetExp(int AmountToGet)
    {
        CurrentExperience += AmountToGet;

        if (CurrentExperience >= ExpLevels[CurrentLevel])
        {
            LevelUp();
        }

        UIController.instance.UpdateExperience(CurrentExperience, ExpLevels[CurrentLevel], CurrentLevel);

        SFXManager.instance.PlaySFXPitched(2);
    }
    public void SpawnExp(Vector3 position, int ExpValue)
    {
        Instantiate(pickup, position, Quaternion.identity).ExpValue = ExpValue;
    }

    void LevelUp()
    {
        CurrentExperience -= ExpLevels[CurrentLevel];

        CurrentLevel++;

        if (CurrentLevel >= ExpLevels.Count)
        {
            CurrentLevel = ExpLevels.Count - 1;
        }

        UIController.instance.LevelUpPanel.SetActive(true);

        Time.timeScale = 0f;
        WeaponsToUpgrade.Clear();
        List<Weapon> AvailableWeapons = new List<Weapon>();
        AvailableWeapons.AddRange(PlayerController.instance.assignedWeapons);
        if (AvailableWeapons.Count > 0)
        {
            int Selected = Random.Range(0, AvailableWeapons.Count);
            WeaponsToUpgrade.Add(AvailableWeapons[Selected]);
            AvailableWeapons.RemoveAt(Selected);
        }

        if (PlayerController.instance.assignedWeapons.Count + PlayerController.instance.FullyLevelledWeapons.Count < PlayerController.instance.MaxWeapons)
        {
            AvailableWeapons.AddRange(PlayerController.instance.unassignedWeapons);
        }

        for (int i = WeaponsToUpgrade.Count; i < 5; i++)
        {
            if (AvailableWeapons.Count > 0)
            {
                int Selected = Random.Range(0, AvailableWeapons.Count);
                WeaponsToUpgrade.Add(AvailableWeapons[Selected]);
                AvailableWeapons.RemoveAt(Selected);
            }
        }

        for (int i = 0; i < WeaponsToUpgrade.Count; i++)
        {
            UIController.instance.LevelUpButtons[i].UpdateButtonDisplay(WeaponsToUpgrade[i]);
        }

        for (int i = 0; i < UIController.instance.LevelUpButtons.Length; i++)
        {
            if (i < WeaponsToUpgrade.Count)
            {
                UIController.instance.LevelUpButtons[i].gameObject.SetActive(true);
            }
            else
            {
                UIController.instance.LevelUpButtons[i].gameObject.SetActive(false);
            }
        }

        PlayerStatController.instance.UpdateDisplay();
    }
}
