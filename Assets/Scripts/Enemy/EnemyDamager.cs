using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamager : MonoBehaviour
{
    public float DamageAmount, LifeTime, GrowSpeed = 5f;
    private Vector3 targetSize;
    public bool ShouldKnockBack;
    public bool destroyParent;

    public bool damageOverTime;
    public float timeBetweenDamage;
    private float damageCounter;

    private List<EnemyController> enemiesInRange = new List<EnemyController>();

    public bool destroyOnImpact;

    public bool DestroyParent;
    // Start is called before the first frame update
    void Start()
    {
        // Удалить объект(световой шар) каждую ## (в секундах)
        //Destroy(gameObject, LifeTime);
        targetSize = transform.localScale;
        transform.localScale = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale = Vector3.MoveTowards(transform.localScale, targetSize, GrowSpeed * Time.deltaTime);

        LifeTime -= Time.deltaTime;

        if (LifeTime <= 0)
        {
            targetSize = Vector3.zero;

            if (transform.localScale.x == 0f)
            {
                Destroy(gameObject);
                if (DestroyParent)
                {
                    Destroy(transform.parent.gameObject);
                }
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (damageOverTime == false)
        {
            if (collision.tag == "Enemy")
            {
                collision.GetComponent<EnemyController>().TakeDamage(DamageAmount, ShouldKnockBack);
                if (destroyOnImpact)
                {
                    Destroy(gameObject);
                }
            }
            else
            {
                if (collision.tag == "Enemy")
                {
                    enemiesInRange.Add(collision.GetComponent<EnemyController>());
                }
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (damageOverTime == true)
        {
            if (collision.tag == "Enemy")
            {
                enemiesInRange.Remove(collision.GetComponent<EnemyController>());
            }
        }
    }
}
