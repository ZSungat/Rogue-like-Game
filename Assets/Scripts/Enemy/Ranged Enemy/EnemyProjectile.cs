using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public float moveSpeed;
    private float damage;
    private float lifeTime;
    private float HitCounter;
    public float HitWaitTime = 1f;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);
        if (PlayerController.instance.gameObject.activeSelf)
        {
            if (HitCounter > 0f)
            {
                HitCounter -= Time.deltaTime;
            }
        }
    }

    public void SetDamage(float damageAmount)
    {
        damage = damageAmount;
    }

    public void SetLifeTime(float lifeTimeAmount)
    {
        lifeTime = lifeTimeAmount;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && HitCounter <= 0f)
        {
            PlayerHealthController.instance.TakeDamage(Mathf.RoundToInt(damage)); // Round to int here for damage calculation
            HitCounter = HitWaitTime;
            Destroy(gameObject);
        }
    }
}
