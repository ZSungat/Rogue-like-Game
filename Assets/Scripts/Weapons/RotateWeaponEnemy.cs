using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateWeaponEnemy : MonoBehaviour
{
    public float rotationSpeed;
    public string enemyTag;
    private Quaternion initialRotation;
    private Transform playerTransform;

    void Start()
    {
        initialRotation = Quaternion.Euler(0, 0, 45); // Default initial rotation of 45 degrees
        playerTransform = PlayerController.instance.transform; // Get reference to the player's transform
    }

    void Update()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);

        if (enemies.Length > 0)
        {
            GameObject closestEnemy = GetClosestEnemy(enemies);
            RotateTowardsEnemy(closestEnemy.transform);
        }
    }

    GameObject GetClosestEnemy(GameObject[] enemies)
    {
        GameObject closestEnemy = null;
        float closestDistance = Mathf.Infinity;
        Vector3 currentPosition = transform.position;

        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(currentPosition, enemy.transform.position);
            if (distanceToEnemy < closestDistance)
            {
                closestEnemy = enemy;
                closestDistance = distanceToEnemy;
            }
        }

        return closestEnemy;
    }

    void RotateTowardsEnemy(Transform enemyTransform)
    {
        Vector3 direction = enemyTransform.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Adjust angle based on object's initial rotation and flipped X-axis of the player
        angle += 45; // Additional 45 degrees in Z-axis

        // Check if player is facing left by checking the scale along the X-axis
        if (playerTransform.localScale.x < 0)
        {
            angle += 85; // Adjust angle for left-facing object
        }

        Quaternion targetRotation = Quaternion.Euler(0, 0, angle);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    }
}
