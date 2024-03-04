using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public List<WeaponStats> Stats;
    public int WeaponLevel;

    [HideInInspector]
    public bool StatsUpdated;

    public Sprite icon;

    public void LevelUp()
    {
        if (WeaponLevel < Stats.Count - 1)
        {
            WeaponLevel++;

            StatsUpdated = true;

            if (WeaponLevel >= Stats.Count - 1)
            {
                PlayerController.instance.FullyLevelledWeapons.Add(this);
                PlayerController.instance.assignedWeapons.Remove(this);
            }
        }
    }
}

[System.Serializable]
public class WeaponStats
{
    public float Speed;
    public int Damage;
    public float Range, TimeBetweenAttacks, Amount, Duration;
    public string UpgradeText;
}
