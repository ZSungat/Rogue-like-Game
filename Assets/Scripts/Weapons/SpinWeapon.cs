using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinWeapon : Weapon
{
    public static SpinWeapon instance;

    public float RotateSpeed, TimeBetweenSpawn;
    private float SpawnCounter;
    public EnemyDamager Damager;

    public Transform holder, LightballToSpawn;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        SetStats();
        //UIController.instance.LevelUpButtons[0].UpdateButtonDisplay(this);
    }

    // Update is called once per frame
    void Update()
    {
        holder.rotation = Quaternion.Euler(0f, 0f, holder.rotation.eulerAngles.z + (RotateSpeed * Time.deltaTime * Stats[WeaponLevel].Speed));
        // Spawn lightball after a certain time.
        SpawnCounter -= Time.deltaTime;
        if (SpawnCounter <= 0)
        {
            SpawnCounter = TimeBetweenSpawn;

            for (int i = 0; i < Stats[WeaponLevel].Amount; i++)
            {
                // Additional check to prevent spawning if max limit is reached
                if (spawnedWeapons.Count >= Stats[WeaponLevel].Amount)
                {
                    Debug.LogWarning("Max weapons reached!");
                    break; // Exit the loop
                }

                // Calculate the angle based on the amount.
                float rot = (360f / Stats[WeaponLevel].Amount) * i;
                // Convert the angle to radians.
                float angleRad = rot * Mathf.Deg2Rad;

                // Calculate the x and y position using the angle.
                float x = Mathf.Cos(angleRad);
                float y = Mathf.Sin(angleRad);

                // Create the position vector.
                Vector2 position = new Vector2(x, y);

                // Convert the Vector2 to a Vector3.
                Vector3 position3D = new Vector3(position.x, position.y, 0);

                // Multiply the position by the range to get the final position.
                Vector3 finalPosition = holder.position + position3D * Stats[WeaponLevel].Range;

                Instantiate(LightballToSpawn, finalPosition, Quaternion.Euler(0f, 0f, rot), holder).gameObject.SetActive(true);
                SFXManager.instance.PlaySFX(7);
            }
        }

        if (StatsUpdated)
        {
            StatsUpdated = false;

            SetStats();
        }
    }

    public void SetStats()
    {
        Damager.DamageAmount = Stats[WeaponLevel].Damage;

        transform.localScale = Vector3.one * Stats[WeaponLevel].Range;


        Damager.LifeTime = Stats[WeaponLevel].Duration;

        SpawnCounter = 0f;
    }
}
