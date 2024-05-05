using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reposition : MonoBehaviour
{
    Collider2D coll;

    void Awake()
    {
        coll = GetComponent<Collider2D>();
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (!collider.CompareTag("Area"))
        {
            return;
        }

        Vector3 PlayerPosition = GameManager.Instance.player.transform.position;
        Vector3 MyPosition = transform.position;
        float dirX = PlayerPosition.x - MyPosition.x;
        float dirY = PlayerPosition.y - MyPosition.y;

        float diffX = Mathf.Abs(dirX);
        float diffY = Mathf.Abs(dirY);

        Vector3 PlayerDirection = GameManager.Instance.player.moveVector;
        dirX = dirX > 0 ? 1 : -1;
        dirY = dirY > 0 ? 1 : -1;

        switch (transform.tag)
        {
            case "Terrain":
                RepositionTerrain(diffX, diffY, dirX, dirY);
                break;
            case "Enemy":
                RepositionEnemy(PlayerDirection);
                break;
            case "Player":
                RepositionPlayer();
                break;
        }
    }

    private void RepositionTerrain(float diffX, float diffY, float dirX, float dirY)
    {
        if (Mathf.Abs(diffX - diffY) <= 0.1f)
        {
            transform.Translate(Vector3.up * dirY * 48);
            transform.Translate(Vector3.right * dirX * 48);
        }
        else if (diffX > diffY)
        {
            transform.Translate(Vector3.right * dirX * 48);
        }
        else if (diffX < diffY)
        {
            transform.Translate(Vector3.up * dirY * 48);
        }
    }

    private void RepositionEnemy(Vector3 PlayerDirection)
    {
        if (coll.enabled)
        {
            transform.Translate(PlayerDirection * 24 + new Vector3(Random.Range(-3f, -3f), Random.Range(-3f, -3f), 0f));
        }
    }

    private void RepositionPlayer()
    {
        if (coll.enabled)
        {
            // Handle player repositioning if needed
        }
    }
}

