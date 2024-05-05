using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthController : MonoBehaviour
{
    public static PlayerHealthController instance;
    public PlayerController PlayerControllerScript;

    private void Awake()
    {
        instance = this;
    }

    public float CurrentHealth, MaxHealth;
    public int NumberOfHearts;

    public Image[] Hearts;
    public Sprite FullHeart;
    public Sprite emptyHeart;

    public GameObject DeathEffect;

    private bool isDead = false;

    // Start is called before the first frame update
    void Start()
    {
        MaxHealth = PlayerStatController.instance.Health[0].Value;
        CurrentHealth = MaxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (CurrentHealth > NumberOfHearts)
        {
            CurrentHealth = NumberOfHearts;
        }

        // Check if CurrentHealth is zero or below to set empty hearts
        if (CurrentHealth <= 0 && !isDead)
        {
            isDead = true;
            StartCoroutine(DeactivatePlayer());
            return; // Exit Update to prevent further heart updates
        }

        // Display hearts based on CurrentHealth
        for (int i = 0; i < Hearts.Length; i++)
        {
            if (i < Mathf.CeilToInt(CurrentHealth))
            {
                Hearts[i].sprite = FullHeart;
            }
            else
            {
                Hearts[i].sprite = emptyHeart;
            }
            if (i < NumberOfHearts)
            {
                Hearts[i].enabled = true;
            }
            else
            {
                Hearts[i].enabled = false;
            }
        }
    }

    IEnumerator DeactivatePlayer()
    {
        yield return new WaitForSeconds(0.1f); // Adjust the delay as needed
        PlayerControllerScript.OnDisable();
        LevelManager.instance.EndLevel();
        Instantiate(DeathEffect, transform.position, transform.rotation);
        gameObject.SetActive(false);
    }

    public void TakeDamage(int DamageToTake)
    {
        CurrentHealth -= DamageToTake;

        if (CurrentHealth <= 0 && !isDead)
        {
            isDead = true;
            StartCoroutine(DeactivatePlayer());
            LevelManager.instance.EndLevel();
        }
    }
}
