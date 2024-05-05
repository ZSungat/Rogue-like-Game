using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateWeaponEnemy : MonoBehaviour
{
    public float rotationSpeed;
    public string enemyTag;
    public float additionalRotation = 0f; // Additional rotation to fine-tune the rotation of the icon

    private Quaternion initialRotation;

    void Start()
    {
        initialRotation = Quaternion.Euler(0, 0, 45); // Default initial rotation of 45 degrees
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

        // Adjust the angle based on initial rotation and flip, and add additional rotation
        angle -= 90 - initialRotation.eulerAngles.z + additionalRotation;

        // Adjust angle for flipped icon
        if (transform.localScale.x < 0)
        {
            angle += 180;
        }

        Quaternion targetRotation = Quaternion.Euler(0, 0, angle);

        // Interpolating rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    }
}