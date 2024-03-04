using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrownWeapon : MonoBehaviour
{
    public float ThrowPower;
    public Rigidbody2D TheRB;
    public float RotateSpeed;

    // Start is called before the first frame update
    void Start()
    {
        TheRB.velocity = new Vector2(Random.Range(-ThrowPower, ThrowPower), ThrowPower);
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Euler(0f, 0f, transform.rotation.eulerAngles.z + (RotateSpeed * 360f * Time.deltaTime * Mathf.Sign(TheRB.velocity.x)));
    }
}
