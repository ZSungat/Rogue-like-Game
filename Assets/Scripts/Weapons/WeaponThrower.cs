using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponThrower : Weapon
{
    public EnemyDamager Damager;

    private float ThrowCounter;

    void Start()
    {
        SetStats();
    }

    // Update is called once per frame
    void Update()
    {
        if (StatsUpdated)
        {
            StatsUpdated = false;
            SetStats();
        }

        ThrowCounter -= Time.deltaTime;
        if (ThrowCounter <= 0)
        {
            ThrowCounter = Stats[WeaponLevel].TimeBetweenAttacks;

            for (int i = 0; i < Stats[WeaponLevel].Amount; i++)
            {
                EnemyDamager newDamager = Instantiate(Damager, transform.position, Quaternion.identity);
                newDamager.gameObject.SetActive(true);
            }

            SFXManager.instance.PlaySFXPitched(5);
        }
    }

    void SetStats()
    {
        Damager.DamageAmount = Stats[WeaponLevel].Damage;
        Damager.LifeTime = Stats[WeaponLevel].Duration;
        Damager.transform.localScale = Vector3.one * Stats[WeaponLevel].Range;
        ThrowCounter = 0f;
    }
}
