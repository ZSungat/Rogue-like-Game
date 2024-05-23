using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseAttackWeapon : Weapon
{
    public EnemyDamager Damager;

    private float attackCounter, direction;
    private bool sfxPlayed = false; // Flag to track if SFX has been played

    void Start()
    {
        SetStats();
    }

    void Update()
    {
        if (StatsUpdated)
        {
            StatsUpdated = false;
            SetStats();
        }

        attackCounter -= Time.deltaTime;
        if (attackCounter <= 0)
        {
            attackCounter = Stats[WeaponLevel].TimeBetweenAttacks;

            direction = Input.GetAxisRaw("Horizontal");

            // Ensure the weapon always faces right
            if (direction != 0)
            {
                Damager.transform.rotation = Quaternion.identity;
            }

            SpawnWeapon();
            RemoveExcessWeapons();

            // Reset the flag after the weapon appears
            sfxPlayed = false;
        }
    }

    void SetStats()
    {
        Damager.DamageAmount = Stats[WeaponLevel].Damage;
        Damager.LifeTime = Stats[WeaponLevel].Duration;
        Damager.transform.localScale = Vector3.one * Stats[WeaponLevel].Range;

        attackCounter = 0f;
    }

    new void RemoveExcessWeapons()
    {
        base.RemoveExcessWeapons(); // Call the RemoveExcessWeapons method from the base class
    }

    new bool CanSpawn()
    {
        return base.CanSpawn(); // Call the CanSpawn method from the base class
    }

    void SpawnWeapon()
    {
        if (CanSpawn())
        {
            Instantiate(Damager, Damager.transform.position, Damager.transform.rotation, transform).gameObject.SetActive(true);

            // Play the SFX only if it hasn't been played already
            if (!sfxPlayed)
            {
                SFXManager.instance.PlaySFXPitched(4);
                sfxPlayed = true;
            }

            for (int i = 1; i < (int)Stats[WeaponLevel].Amount; i++)
            {
                float rot = (360f / (int)Stats[WeaponLevel].Amount) * i;
                Instantiate(Damager, Damager.transform.position, Quaternion.Euler(0f, 0f, Damager.transform.rotation.eulerAngles.z + rot), transform).gameObject.SetActive(true);

                // Play the SFX only if it hasn't been played already
                if (!sfxPlayed)
                {   
                    SFXManager.instance.PlaySFXPitched(9);
                    sfxPlayed = true; // Set the flag to true after playing the SFX
                }
            }
        }
    }
}
