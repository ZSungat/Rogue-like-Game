using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpPickup : MonoBehaviour
{
    public int ExpValue;
    private bool MovingToPlayer;
    public float MoveSpeed;
    public float TimeBetweenChecks = .2f;
    private float checkCounter;
    private PlayerController player;

    // Start is called before the first frame update
    void Start()
    {
        player = PlayerHealthController.instance.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (MovingToPlayer == true)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, MoveSpeed * Time.deltaTime);
        }
        else
        {
            checkCounter -= Time.deltaTime;
            if (checkCounter <= 0)
            {
                checkCounter = TimeBetweenChecks;

                if (Vector3.Distance(transform.position, player.transform.position) < player.PickupRange)
                {
                    MovingToPlayer = true;
                    MoveSpeed += player.MoveSpeed;
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            ExperienceLevelController.instance.GetExp(ExpValue);
            Destroy(gameObject);
        }
    }
}
