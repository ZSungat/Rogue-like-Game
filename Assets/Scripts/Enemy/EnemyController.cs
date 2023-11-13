using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Rigidbody2D rb;
    private Transform target;
    private Animator anim;
    private SpriteRenderer sr;

    public float moveSpeed;
    private float HitCounter;
    private float knockBackCounter;

    public float damage;
    public float HitWaitTime = 1f;
    public float health = 5f;
    public float knockBackTime = .5f;
    public int ExpToGive = 1;





    // Start is called before the first frame update

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }
    void Start()
    {
        // target = FindObjectOfType<PlayerMovement>().transform;
        target = PlayerHealthController.instance.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerMovement.instance.gameObject.activeSelf == true)
        {
            if (knockBackCounter > 0)
            {
                knockBackCounter -= Time.deltaTime;

                if (moveSpeed > 0)
                {
                    moveSpeed = -moveSpeed * 2f;
                    anim.SetBool("IsHit", true);
                }

                if (knockBackCounter <= 0)
                {
                    moveSpeed = Mathf.Abs(moveSpeed * .5f);
                    anim.SetBool("IsHit", false);
                }
            }
            rb.velocity = (target.position - transform.position).normalized * moveSpeed;

            if (HitCounter > 0f)
            {
                HitCounter -= Time.deltaTime;
            }
            // else
            // {
            //     rb.velocity = Vector2.zero;
            // }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && HitCounter <= 0f)
        {
            PlayerHealthController.instance.TakeDamage(damage);

            HitCounter = HitWaitTime;
        }
    }
    private void OnEnable()
    {
        anim.SetBool("IsMoving", true);
    }
    private void OnDisable()
    {
        anim.SetBool("IsMoving", false);
    }
    private void LateUpdate()
    {
        sr.flipX = target.position.x < rb.position.x;
    }

    public void TakeDamage(float damageToTake)
    {
        health -= damageToTake;

        if (health <= 0)
        {
            anim.SetBool("IsDead", true);
            Destroy(gameObject);
            ExperienceLevelController.instance.SpawnExp(transform.position, ExpToGive);
        }

        DamageNumberController.instance.SpawnDamage(damageToTake, transform.position);
    }

    public void TakeDamage(float damageToTake, bool ShouldKnockBack)
    {
        TakeDamage(damageToTake);

        if (ShouldKnockBack == true)
        {
            knockBackCounter = knockBackTime;
        }
    }
}