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
    private float initialMoveSpeed;
    private float hitCounter;
    private float knockBackCounter;
    public int damage;
    public float hitWaitTime = 1f;
    public float health = 5f;
    public float maxHealth = 5f;
    public float knockBackTime = .5f;
    public int expToGive = 1;
    public int coinValue = 1;
    public float coinDropRate = .5f;
    private EnemyHealthBar healthBar;
    [SerializeField] private Vector3 healthBarOffset = new Vector3(0, 1, 0);

    // -----------------------------Ranged Enemy -----------------------------//
    public bool isRangedEnemy;
    public float stoppingDistance;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    void Start()
    {
        target = PlayerHealthController.instance.transform;
        rb = GetComponent<Rigidbody2D>();
        initialMoveSpeed = moveSpeed;
    }

    private void Tick()
    {
        if (PlayerController.instance.gameObject.activeSelf)
        {
            if (knockBackCounter > 0)
            {
                knockBackCounter -= Time.deltaTime;
                if (knockBackCounter <= 0)
                {
                    moveSpeed = initialMoveSpeed;
                    anim.SetBool("IsHit", false);
                }
            }
            else
            {
                float distanceToTarget = Vector2.Distance(transform.position, target.position);
                Vector2 direction;

                if (!isRangedEnemy)
                {
                    direction = (target.position - transform.position).normalized;
                    rb.velocity = direction * moveSpeed;
                    anim.SetBool("IsMoving", true);
                }
                else
                {
                    if (distanceToTarget > stoppingDistance)
                    {
                        direction = (target.position - transform.position).normalized;
                        rb.velocity = direction * moveSpeed;
                        anim.SetBool("IsMoving", true);
                    }
                    else if (distanceToTarget < stoppingDistance)
                    {
                        direction = (transform.position - target.position).normalized;
                        rb.velocity = direction * moveSpeed; // Move away from the player
                        anim.SetBool("IsMoving", true);
                    }
                    else
                    {
                        rb.velocity = Vector2.zero;
                        anim.SetBool("IsMoving", false);
                    }
                }
            }

            if (hitCounter > 0f)
            {
                hitCounter -= Time.deltaTime;
            }
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && hitCounter <= 0f)
        {
            PlayerHealthController.instance.TakeDamage(damage);
            hitCounter = hitWaitTime;
        }
    }

    private void OnEnable()
    {
        Ticker.OnTickAction += Tick;
        anim.SetBool("IsMoving", true);
    }

    private void OnDisable()
    {
        Ticker.OnTickAction -= Tick;
        anim.SetBool("IsMoving", false);
    }

    private void LateUpdate()
    {
        sr.flipX = target.position.x < rb.position.x;
    }

    public void TakeDamage(int damageToTake)
    {
        health -= damageToTake;
        healthBar.UpdateHealthBar(health / maxHealth);
        if (health <= 0)
        {
            anim.SetBool("IsDead", true); // Trigger the IsDead animation
            Destroy(gameObject);
            ExperienceLevelController.instance.SpawnExp(transform.position, expToGive);
            if (Random.value <= coinDropRate)
            {
                CoinController.instance.DropCoin(transform.position, coinValue);
            }
            SFXManager.instance.PlaySFXPitched(0);
            Destroy(healthBar.gameObject); // Destroy health bar when enemy dies
        }
        else
        {
            SFXManager.instance.PlaySFXPitched(1);
        }

        DamageNumberController.instance.SpawnDamage(damageToTake, transform.position);
    }

    public void TakeDamage(int damageToTake, bool shouldKnockBack)
    {
        TakeDamage(damageToTake);

        if (shouldKnockBack)
        {
            ApplyKnockBack();
        }
    }

    private void ApplyKnockBack()
    {
        if (knockBackCounter <= 0)
        {
            knockBackCounter = knockBackTime;
            moveSpeed = -initialMoveSpeed * 2f; // Apply knockback by reversing and increasing speed
            anim.SetBool("IsHit", true); // Trigger the IsHit animation
        }
    }

    public void SetHealthBar(EnemyHealthBar newHealthBar)
    {
        healthBar = newHealthBar;
        healthBar.Initialize(transform, healthBarOffset);
        healthBar.UpdateHealthBar(health / maxHealth);
    }
}
