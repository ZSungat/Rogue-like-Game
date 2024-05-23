using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : MonoBehaviour
{
    public EnemyProjectile projectilePrefab;

    private float shotCounter;

    public float weaponRange = 10f;

    // Customizable stats
    public int damageAmount = 1;
    public float projectileSpeed = 5f;
    public float timeBetweenAttacks = 1f;
    public int projectilesPerAttack = 3;
    public float projectileLifetime = 5f; // Example duration

    void Start()
    {
        // Initialize weapon stats
        shotCounter = 0f;
    }

    void Update()
    {
        shotCounter -= Time.deltaTime;
        if (shotCounter <= 0)
        {
            shotCounter = timeBetweenAttacks;

            GameObject player = GameObject.FindGameObjectWithTag("Player"); // Find the player object by tag
            if (player != null)
            {
                float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
                if (distanceToPlayer <= weaponRange)
                {
                    for (int i = 0; i < projectilesPerAttack; i++)
                    {
                        FireProjectile(player.transform.position);
                    }
                    SFXManager.instance.PlaySFXPitched(6);
                }
            }
        }
    }

    void FireProjectile(Vector3 targetPosition)
    {
        Vector3 direction = (targetPosition - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;

        EnemyProjectile newProjectile = Instantiate(projectilePrefab, transform.position, Quaternion.Euler(0, 0, angle));
        newProjectile.moveSpeed = projectileSpeed;
        newProjectile.SetDamage(damageAmount); // Assuming a method to set damage
        newProjectile.SetLifeTime(projectileLifetime); // Assuming a method to set lifeTime
        newProjectile.gameObject.SetActive(true);
    }
}
