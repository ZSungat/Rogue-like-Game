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
        // switch (transform.tag)
        // {
        //     case "Terrain":
        //         if (diffX > diffY)
        //         {
        //             transform.Translate(Vector3.right * dirX * 48);
        //         }
        //         else if (diffX < diffY)
        //         {
        //             transform.Translate(Vector3.up * dirY * 48);
        //         }
        //         break;
        //     case "Enemy":
        //         if (coll.enabled)
        //         {
        //             transform.Translate(PlayerDirection * 24 + new Vector3(Random.Range(-3f, -3f), Random.Range(-3f, -3f), 0f));
        //         }
        //         break;
        // }
        switch (transform.tag)
        {
            case "Terrain":
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
                break;
            case "Enemy":
                if (coll.enabled)
                {
                    transform.Translate(PlayerDirection * 24 + new Vector3(Random.Range(-3f, -3f), Random.Range(-3f, -3f), 0f));
                }
                break;
        }
    }
}
