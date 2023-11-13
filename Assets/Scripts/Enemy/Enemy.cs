using System.Reflection;
using System.Runtime.CompilerServices;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed, health, maxHealth, damage;
    public RuntimeAnimatorController[] animCon;
    public Rigidbody2D target;
    public float HitWaitTime = 1f;
    private float HitCounter;
    bool IsLive;

    Rigidbody2D rb;
    Animator anim;
    SpriteRenderer sr;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }
    void Update()
    {
        if (HitCounter > 0f)
        {
            HitCounter -= Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        if (!IsLive)
        {
            return;
        }
        Vector2 dirVec = target.position - rb.position;
        Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + nextVec);
        rb.velocity = Vector2.zero;
        // if (dirVec.x > 0)
        // {
        //     transform.localScale = Vector3.one;
        // }
        // else
        // {
        //     transform.localScale = new Vector3(-1f, 1f, 1f);
        // }
    }
    private void OnDisable()
    {
        if (!IsLive)
        {
            return;
        }
        anim.SetBool("IsRunning", false);
        Vector2 nextVec = Vector2.zero;
        Vector2 dirVec = Vector2.zero;
    }
    private void OnEnable()
    {
        anim.SetBool("IsRunning", true);
        target = GameManager.Instance.player.GetComponent<Rigidbody2D>();
        IsLive = true;
        health = maxHealth;
    }

    private void LateUpdate()
    {
        if (!IsLive)
        {
            return;
        }
        sr.flipX = target.position.x < rb.position.x;
    }
    // public void Init(SpawnData data)
    // {
    //     anim.runtimeAnimatorController = animCon[data.spriteType];
    //     speed = data.speed;
    //     maxHealth = data.health;
    //     health = data.health;
    // }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // if (!collision.CompareTag("Bullet"))
        // {
        //     return;
        // }
        // health -= collision.GetComponent<Bullet>().damage;

        // if (health > 0)
        // {
        //     health = Closure(target.position * rb.position);
        // }
        // else
        // {
        //     //.. Die
        //     Dead();
        // }
        if (collision.gameObject.tag == "Player" && HitCounter <= 0f)
        {
            PlayerHealthController.instance.TakeDamage(damage);

            HitCounter = HitWaitTime;
        }
    }
    void Dead()
    {
        gameObject.SetActive(false);
    }
}
