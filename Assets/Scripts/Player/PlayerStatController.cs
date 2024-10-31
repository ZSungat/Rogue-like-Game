using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatController : MonoBehaviour
{
    public static PlayerStatController instance;
    private void Awake()
    {
        instance = this;
    }

    public int MoveSpeedLevelCount, HealthLevelCount, PickupRangeLevelCount;
    public List<PlayerStatValue> MoveSpeed, Health, PickupRange, MaxWeapons;

    public int MoveSpeedLevel, HealthLevel, PickupRangeLevel, MaxWeaponsLevel;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = MoveSpeed.Count - 1; i < MoveSpeedLevelCount; i++)
        {
            MoveSpeed.Add(new PlayerStatValue(MoveSpeed[i].Cost + MoveSpeed[1].Cost, MoveSpeed[i].Value + (MoveSpeed[1].Value - MoveSpeed[0].Value)));
        }

        for (int i = Health.Count - 1; i < HealthLevelCount; i++)
        {
            Health.Add(new PlayerStatValue(Health[i].Cost + Health[1].Cost, Health[i].Value + (Health[1].Value - Health[0].Value)));
        }

        for (int i = PickupRange.Count - 1; i < PickupRangeLevelCount; i++)
        {
            PickupRange.Add(new PlayerStatValue(PickupRange[i].Cost + PickupRange[1].Cost, PickupRange[i].Value + (PickupRange[1].Value - PickupRange[0].Value)));
        }
    }
    void Update()
    {
        if (UIController.instance.LevelUpPanel.activeSelf)
        {
            UpdateDisplay();
        }
    }
    public void UpdateDisplay()
    {
        if (MoveSpeedLevel < MoveSpeed.Count - 1)
        {
            UIController.instance.MoveSpeedUpgradeDisplay.UpdateDisplay(MoveSpeed[MoveSpeedLevel + 1].Cost, MoveSpeed[MoveSpeedLevel].Value, MoveSpeed[MoveSpeedLevel + 1].Value);
        }
        else
        {
            UIController.instance.MoveSpeedUpgradeDisplay.ShowMaxLevel();
        }

        if (HealthLevel < Health.Count - 1)
        {
            UIController.instance.HealthUpgradeDisplay.UpdateDisplay(Health[HealthLevel + 1].Cost, Health[HealthLevel].Value, Health[HealthLevel + 1].Value);
        }
        else
        {
            UIController.instance.HealthUpgradeDisplay.ShowMaxLevel();
        }

        if (PickupRangeLevel < PickupRange.Count - 1)
        {
            UIController.instance.PickupRangeUpgradeDisplay.UpdateDisplay(PickupRange[PickupRangeLevel + 1].Cost, PickupRange[PickupRangeLevel].Value, PickupRange[PickupRangeLevel + 1].Value);
        }
        else
        {
            UIController.instance.PickupRangeUpgradeDisplay.ShowMaxLevel();
        }

        if (MaxWeaponsLevel < MaxWeapons.Count - 1)
        {
            UIController.instance.MaxWeaponsUpgradeDisplay.UpdateDisplay(MaxWeapons[MaxWeaponsLevel + 1].Cost, MaxWeapons[MaxWeaponsLevel].Value, MaxWeapons[MaxWeaponsLevel + 1].Value);
        }
        else
        {
            UIController.instance.MaxWeaponsUpgradeDisplay.ShowMaxLevel();
        }
    }
    public void PurchaseMoveSpeed()
    {
        MoveSpeedLevel++;
        CoinController.instance.SpendCoins(MoveSpeed[MoveSpeedLevel].Cost);
        UpdateDisplay();

        PlayerController.instance.MoveSpeed = MoveSpeed[MoveSpeedLevel].Value;
    }

    public void PurchaseHealth()
    {
        int oldHealth = Mathf.RoundToInt(Health[HealthLevel].Value);
        HealthLevel++;
        int newHealth = Mathf.RoundToInt(Health[HealthLevel].Value);

        CoinController.instance.SpendCoins(Health[HealthLevel].Cost);
        UpdateDisplay();

        // Use the new integer-based UpgradeHealth method
        PlayerHealthController.instance.UpgradeHealth(oldHealth, newHealth);
    }


    public void PurchasePickupRange()
    {
        PickupRangeLevel++;
        CoinController.instance.SpendCoins(PickupRange[PickupRangeLevel].Cost);
        UpdateDisplay();

        PlayerController.instance.PickupRange = PickupRange[PickupRangeLevel].Value;
    }

    public void PurchaseMaxWeapons()
    {
        MaxWeaponsLevel++;
        CoinController.instance.SpendCoins(MaxWeapons[MaxWeaponsLevel].Cost);
        UpdateDisplay();

        PlayerController.instance.MaxWeapons = Mathf.RoundToInt(MaxWeapons[MaxWeaponsLevel].Value);
    }
}
[System.Serializable]
public class PlayerStatValue
{
    public int Cost;
    public float Value;

    public PlayerStatValue(int NewCost, float NewValue)
    {
        Cost = NewCost;
        Value = NewValue;
    }
}
