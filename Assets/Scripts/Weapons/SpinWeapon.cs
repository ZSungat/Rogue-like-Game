using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinWeapon : Weapon
{
    public float RotateSpeed;
    public float TimeBetweenSpawn;
    private float SpawnCounter;
    public EnemyDamager Damager;

    public Transform holder, LightballToSpawn;

    // Start is called before the first frame update
    void Start()
    {
        SetStats();
    }

    // Update is called once per frame
    void Update()
    {
        holder.rotation = Quaternion.Euler(0f, 0f, holder.rotation.eulerAngles.z + (RotateSpeed * Time.deltaTime * Stats[WeaponLevel].Speed));
        // Создавать световой шар через # времени.
        SpawnCounter -= Time.deltaTime;
        if (SpawnCounter <= 0)
        {
            SpawnCounter = TimeBetweenSpawn;

            for (int i = 0; i < Stats[WeaponLevel].Amount; i++)
            {
                float rot = (360f / Stats[WeaponLevel].Amount) * i;

                Instantiate(LightballToSpawn, LightballToSpawn.position, Quaternion.Euler(0f, 0f, rot), holder).gameObject.SetActive(true);

                //SFXManager.instance.PlaySFX(8);
            }

        }

        if (StatsUpdated == true)
        {
            StatsUpdated = false;
            SetStats();
        }
    }

    public void SetStats()
    {
        Damager.DamageAmount = Stats[WeaponLevel].Damage;

        transform.localScale = Vector3.one * Stats[WeaponLevel].Range;

        TimeBetweenSpawn = Stats[WeaponLevel].TimeBetweenAttacks;

        Damager.LifeTime = Stats[WeaponLevel].Duration;

        SpawnCounter = 0f;
    }
}
