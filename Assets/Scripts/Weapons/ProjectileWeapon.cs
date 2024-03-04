using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileWeapon : Weapon
{
    public EnemyDamager Damager;
    public Projectile projectile;

    private float ShotCounter;

    public float WeaponRange;
    public LayerMask WhatIsEnemy;
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
        ShotCounter -= Time.deltaTime;
        if (ShotCounter <= 0)
        {
            ShotCounter = Stats[WeaponLevel].TimeBetweenAttacks;

            Collider2D[] Enemies = Physics2D.OverlapCircleAll(transform.position, WeaponRange * Stats[WeaponLevel].Range, WhatIsEnemy);
            if (Enemies.Length > 0)
            {
                for (int i = 0; i < Stats[WeaponLevel].Amount; i++)
                {
                    Vector3 TargetPosition = Enemies[Random.Range(0, Enemies.Length)].transform.position;

                    Vector3 Direction = TargetPosition - transform.position;
                    float Angle = Mathf.Atan2(Direction.y, Direction.x) * Mathf.Rad2Deg;
                    Angle -= 90;
                    projectile.transform.rotation = Quaternion.AngleAxis(Angle, Vector3.forward);

                    Instantiate(projectile, projectile.transform.position, projectile.transform.rotation).gameObject.SetActive(true);
                }

                // SFXManager.instance.PlaySFXPitched(6);
            }
        }
    }
    void SetStats()
    {
        Damager.DamageAmount = Stats[WeaponLevel].Damage;
        Damager.LifeTime = Stats[WeaponLevel].Duration;

        Damager.transform.localScale = Vector3.one * Stats[WeaponLevel].Range;

        ShotCounter = 0f;

        projectile.moveSpeed = Stats[WeaponLevel].Speed;
    }
}
