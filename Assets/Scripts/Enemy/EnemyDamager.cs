using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamager : MonoBehaviour
{
    public int DamageAmount;

    public float LifeTime, GrowSpeed = 5f;
    public float TimeBetweenDamage;
    private float DamageCounter;

    private Vector3 TargetSize;

    public bool ShouldKnockBack;
    public bool DestroyParent;
    public bool DamageOverTime;
    public bool DestroyOnImpact;

    private List<EnemyController> EnemiesInRange = new List<EnemyController>();

    // Start is called before the first frame update
    void Start()
    {
        //Destroy(gameObject, LifeTime);

        TargetSize = transform.localScale;
        transform.localScale = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale = Vector3.MoveTowards(transform.localScale, TargetSize, GrowSpeed * Time.deltaTime);

        LifeTime -= Time.deltaTime;

        if (LifeTime <= 0)
        {
            TargetSize = Vector3.zero;

            if (transform.localScale.x == 0f)
            {
                Destroy(gameObject);

                if (DestroyParent)
                {
                    Destroy(transform.parent.gameObject);
                }
            }
        }

        if (DamageOverTime)
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
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!DamageOverTime)
        {
            if (collision.tag == "Enemy")
            {
                collision.GetComponent<EnemyController>().TakeDamage(DamageAmount, ShouldKnockBack);

                if (DestroyOnImpact)
                {
                    Destroy(gameObject);
                }
            }
        }
        else
        {
            if (collision.tag == "Enemy")
            {
                EnemiesInRange.Add(collision.GetComponent<EnemyController>());
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (DamageOverTime)
        {
            if (collision.tag == "Enemy")
            {
                EnemiesInRange.Remove(collision.GetComponent<EnemyController>());
            }
        }
    }
}
