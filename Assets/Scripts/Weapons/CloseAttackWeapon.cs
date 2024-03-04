using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseAttackWeapon : Weapon
{
    public EnemyDamager Damager;

    private float AttackCounter, Direction;
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
        AttackCounter -= Time.deltaTime;
        if (AttackCounter <= 0)
        {
            AttackCounter = Stats[WeaponLevel].TimeBetweenAttacks;

            Direction = Input.GetAxisRaw("Horizontal");

            if (Direction != 0)
            {
                if (Direction > 0)
                {
                    Damager.transform.rotation = Quaternion.identity;
                }
                else
                {
                    Damager.transform.rotation = Quaternion.Euler(0f, 0f, 180f);
                }

            }

            Instantiate(Damager, Damager.transform.position, Damager.transform.rotation, transform).gameObject.SetActive(true);

            for (int i = 1; i < Stats[WeaponLevel].Amount; i++)
            {
                float rot = (360f / Stats[WeaponLevel].Amount) * i;

                Instantiate(Damager, Damager.transform.position, Quaternion.Euler(0f, 0f, Damager.transform.rotation.eulerAngles.z + rot), transform).gameObject.SetActive(true);

            }

            // SFXManager.instance.PlaySFXPitched(9);
        }
    }
    void SetStats()
    {
        Damager.DamageAmount = Stats[WeaponLevel].Damage;
        Damager.LifeTime = Stats[WeaponLevel].Duration;

        Damager.transform.localScale = Vector3.one * Stats[WeaponLevel].Range;

        AttackCounter = 0f;
    }
}
