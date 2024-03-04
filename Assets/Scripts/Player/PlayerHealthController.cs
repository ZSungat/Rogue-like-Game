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
        for (int i = 0; i < Hearts.Length; i++)
        {
            if (i < CurrentHealth)
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

    public void TakeDamage(int DamageToTake)
    {
        CurrentHealth -= DamageToTake;

        if (CurrentHealth <= 0)
        {
            CurrentHealth = Mathf.FloorToInt(CurrentHealth--);

            PlayerControllerScript.OnDisable();

            LevelManager.instance.EndLevel();

            Instantiate(DeathEffect, transform.position, transform.rotation);

            Time.timeScale = 0;
            gameObject.SetActive(false);
        }
    }
}
