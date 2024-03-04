using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageNumber : MonoBehaviour
{
    public TMP_Text DamageText;

    public float LifeTime;
    private float LifeCounter;

    public float floatSpeed = 1f;

    // Update is called once per frame
    void Update()
    {
        if (LifeCounter > 0)
        {
            LifeCounter -= Time.deltaTime;

            if (LifeCounter <= 0)
            {
                DamageNumberController.instance.PlaceInPool(this);
            }
        }

        transform.position += Vector3.up * floatSpeed * Time.deltaTime;
    }

    public void Setup(int DamageDisplay)
    {
        LifeCounter = LifeTime;
        DamageText.text = DamageDisplay.ToString();
    }
}
