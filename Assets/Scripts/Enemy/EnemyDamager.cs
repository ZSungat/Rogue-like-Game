using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamager : MonoBehaviour
{
    public int DamageAmount;
    public float LifeTime, GrowSpeed = 5f, TimeBetweenDamage;
    public bool ShouldKnockBack, DestroyParent, DamageOverTime, DestroyOnImpact;
    public Vector2 GrowthSize; // Vector2 holding Min Size and Max Size

    private float DamageCounter;
    private Vector3 TargetSize;
    private List<EnemyController> EnemiesInRange = new List<EnemyController>();

    void Start()
    {
        transform.localScale = Vector3.one * GrowthSize.x;
        TargetSize = Vector3.one * GrowthSize.y;
    }

    void Update()
    {
        GrowProjectile();
        HandleLifeTime();

        if (DamageOverTime)
        {
            HandleDamageOverTime();
        }
    }

    void GrowProjectile()
    {
        TargetSize = Vector3.Max(Vector3.Min(TargetSize, Vector3.one * GrowthSize.y), Vector3.one * GrowthSize.x);

        if (transform.localScale.magnitude < TargetSize.magnitude)
        {
            transform.localScale = Vector3.MoveTowards(transform.localScale, TargetSize, GrowSpeed * Time.deltaTime);
        }
    }

    void HandleLifeTime()
    {
        LifeTime -= Time.deltaTime;

        if (LifeTime <= 0)
        {
            DestroyProjectile();
        }
    }

    void HandleDamageOverTime()
    {
        DamageCounter -= Time.deltaTime;

        if (DamageCounter <= 0)
        {
            DamageCounter = TimeBetweenDamage;

            for (int i = 0; i < EnemiesInRange.Count; i++)
            {
                if (EnemiesInRange[i] != null)
                {
                    EnemiesInRange[i].TakeDamage(DamageAmount, ShouldKnockBack);
                }
                else
                {
                    EnemiesInRange.RemoveAt(i);
                    i--;
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            EnemyController enemy = collision.GetComponent<EnemyController>();

            if (enemy != null)
            {
                if (!DamageOverTime)
                {
                    enemy.TakeDamage(DamageAmount, ShouldKnockBack);

                    if (DestroyOnImpact)
                    {
                        DestroyProjectile();
                    }
                }
                else
                {
                    EnemiesInRange.Add(enemy);
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (DamageOverTime && collision.CompareTag("Enemy"))
        {
            EnemyController enemy = collision.GetComponent<EnemyController>();
            if (enemy != null)
            {
                EnemiesInRange.Remove(enemy);
            }
        }
    }

    private void DestroyProjectile()
    {
        Destroy(gameObject);

        if (DestroyParent && transform.parent != null)
        {
            Destroy(transform.parent.gameObject);
        }
    }
}
