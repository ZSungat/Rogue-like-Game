using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickUp : MonoBehaviour
{
    public int CoinAmount = 1;

    private bool MovingToPlayer;
    public float MoveSpeed;

    public float TimeBetweenChecks = .2f;
    private float CheckCounter;

    private PlayerController Player;

    // Start is called before the first frame update
    void Start()
    {
        Player = PlayerController.instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (MovingToPlayer)
        {
            transform.position = Vector3.MoveTowards(transform.position, Player.transform.position, MoveSpeed * Time.deltaTime);
        }
        else
        {
            CheckCounter -= Time.deltaTime;
            if (CheckCounter <= 0)
            {
                CheckCounter = TimeBetweenChecks;

                if (Vector3.Distance(transform.position, Player.transform.position) < Player.PickupRange)
                {
                    MovingToPlayer = true;
                    MoveSpeed += Player.MoveSpeed;
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            CoinController.instance.AddCoins(CoinAmount);

            Destroy(gameObject);
        }
    }
}

