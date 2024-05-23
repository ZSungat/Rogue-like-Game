using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public List<WeaponStats> Stats;
    public int WeaponLevel;

    [HideInInspector]
    public bool StatsUpdated;

    public Sprite icon;

    public List<GameObject> spawnedWeapons = new List<GameObject>();

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

    public bool CanSpawn()
    {
        bool canSpawn = spawnedWeapons.Count < Stats[WeaponLevel].Amount;
        return canSpawn;
    }


    public void SpawnWeapon(GameObject weaponPrefab, Vector3 position, Quaternion rotation)
    {
        if (CanSpawn())
        {
            GameObject spawnedWeapon = Instantiate(weaponPrefab, position, rotation, transform);
            spawnedWeapons.Add(spawnedWeapon);
        }
        else
        {
            Debug.LogWarning("Max weapons reached!");
        }
    }

    public void RemoveExcessWeapons()
    {
        if (spawnedWeapons.Count > Stats[WeaponLevel].Amount)
        {
            int excessCount = spawnedWeapons.Count - (int)Stats[WeaponLevel].Amount;
            for (int i = 0; i < excessCount; i++)
            {
                GameObject excessWeapon = spawnedWeapons[0];
                spawnedWeapons.RemoveAt(0);
                Destroy(excessWeapon);
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
