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

        ShotCounter -= Time.deltaTime;
        if (ShotCounter <= 0)
        {
            ShotCounter = Stats[WeaponLevel].TimeBetweenAttacks;

            Collider2D[] Enemies = Physics2D.OverlapCircleAll(transform.position, WeaponRange * Stats[WeaponLevel].Range, WhatIsEnemy);
            if (Enemies.Length > 0)
            {
                for (int i = 0; i < Stats[WeaponLevel].Amount; i++)
                {
                    Transform closestEnemy = FindClosestEnemy(Enemies);
                    if (closestEnemy != null)
                    {
                        FireProjectile(closestEnemy.position);
                        Enemies = RemoveEnemyFromArray(Enemies, closestEnemy);
                    }
                }
                SFXManager.instance.PlaySFXPitched(6);
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

    Transform FindClosestEnemy(Collider2D[] enemies)
    {
        float closestDistance = Mathf.Infinity;
        Transform closestEnemy = null;

        foreach (Collider2D enemy in enemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestEnemy = enemy.transform;
            }
        }

        return closestEnemy;
    }

    Collider2D[] RemoveEnemyFromArray(Collider2D[] array, Transform enemyToRemove)
    {
        List<Collider2D> tempList = new List<Collider2D>(array);
        Collider2D enemyCollider = enemyToRemove.GetComponent<Collider2D>();
        tempList.Remove(enemyCollider);
        return tempList.ToArray();
    }

    void FireProjectile(Vector3 targetPosition)
    {
        Vector3 direction = targetPosition - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        angle -= 90;
        projectile.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        Instantiate(projectile, projectile.transform.position, projectile.transform.rotation).gameObject.SetActive(true);
    }
}
