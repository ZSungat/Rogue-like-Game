using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneWeapon : Weapon
{
    public EnemyDamager Damager;

    private float SpawnTime, SpawnCounter;
    // Start is called before the first frame update
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

        SpawnCounter -= Time.deltaTime;
        if (SpawnCounter <= 0f)
        {
            SpawnCounter = SpawnTime;
            Instantiate(Damager, Damager.transform.position, Quaternion.identity, transform).gameObject.SetActive(true);
        }
    }
    public void SetStats()
    {
        Damager.DamageAmount = Stats[WeaponLevel].Damage;

        Damager.LifeTime = Stats[WeaponLevel].Duration;

        Damager.TimeBetweenDamage = Stats[WeaponLevel].Speed;

        Damager.transform.localScale = Vector3.one * Stats[WeaponLevel].Range;

        SpawnTime = Stats[WeaponLevel].TimeBetweenAttacks;

        SpawnCounter = 0f;
    }
}
