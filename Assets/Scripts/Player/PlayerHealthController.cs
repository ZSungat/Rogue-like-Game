using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHealthController : MonoBehaviour
{
    #region Singleton
    public static PlayerHealthController instance;
    public PlayerController PlayerControllerScript;

    private void Awake()
    {
        instance = this;
    }
    #endregion

    #region Health Variables
    [Header("Health Settings")]
    public int CurrentHealth;
    public int MaxHealth;
    public int NumberOfHearts;

    [Header("UI Elements")]
    public Image[] Hearts;
    public Sprite FullHeart;
    public Sprite emptyHeart;

    [Header("Death Effects")]
    public GameObject DeathEffect;
    #endregion

    private bool isDead = false;

    void Start()
    {
        InitializeHealth();
    }

    void Update()
    {
        if (!isDead)
        {
            UpdateHealthDisplay();
        }
    }

    private void InitializeHealth()
    {
        // Convert float value to int using Round
        MaxHealth = Mathf.RoundToInt(PlayerStatController.instance.Health[0].Value);
        CurrentHealth = MaxHealth;
        NumberOfHearts = MaxHealth;
        UpdateHealthDisplay();
    }

    private void UpdateHealthDisplay()
    {
        // Ensure CurrentHealth doesn't exceed NumberOfHearts
        CurrentHealth = Mathf.Clamp(CurrentHealth, 0, NumberOfHearts);

        // Check for death
        if (CurrentHealth <= 0 && !isDead)
        {
            HandleDeath();
            return;
        }

        // Update heart displays
        for (int i = 0; i < Hearts.Length; i++)
        {
            if (i < NumberOfHearts)
            {
                Hearts[i].enabled = true;
                Hearts[i].sprite = (i < CurrentHealth) ? FullHeart : emptyHeart;
            }
            else
            {
                Hearts[i].enabled = false;
            }
        }
    }

    private void HandleDeath()
    {
        isDead = true;
        StartCoroutine(DeactivatePlayer());
    }

    IEnumerator DeactivatePlayer()
    {
        yield return new WaitForSeconds(0.1f);
        PlayerControllerScript.OnDisable();
        LevelManager.instance.EndLevel();
        SFXManager.instance.PlaySFX(11);
        Instantiate(DeathEffect, transform.position, transform.rotation);
        gameObject.SetActive(false);
    }

    public void TakeDamage(int DamageToTake)
    {
        if (isDead) return;

        CurrentHealth -= DamageToTake;
        SFXManager.instance.PlaySFX(10);

        if (CurrentHealth <= 0)
        {
            HandleDeath();
        }

        UpdateHealthDisplay();
    }

    // Updated method to handle integer health upgrades
    public void UpgradeHealth(int oldMaxHealth, int newMaxHealth)
    {
        // Calculate health difference
        int healthIncrease = newMaxHealth - oldMaxHealth;

        // Update max health
        MaxHealth = newMaxHealth;
        NumberOfHearts = MaxHealth;

        // Increase current health by the same amount
        CurrentHealth += healthIncrease;

        // Make sure current health doesn't exceed max health
        CurrentHealth = Mathf.Clamp(CurrentHealth, 1, MaxHealth);

        UpdateHealthDisplay();
    }
}