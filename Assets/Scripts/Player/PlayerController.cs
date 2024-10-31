using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    public float MoveSpeed;
    public float PickupRange;
    public int MaxWeapons;

    [Header("Components")]
    private CustomInput input;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Animator animator;

    [Header("Weapons")]
    [HideInInspector] public List<Weapon> FullyLevelledWeapons = new List<Weapon>();
    public List<Weapon> unassignedWeapons, assignedWeapons;

    [Header("Character Management")]
    [SerializeField] private CharacterManager characterManager;
    [SerializeField] private CharacterManager uiCharacterManager;

    [Header("Movement")]
    private bool isFacingLeft = false;
    public Vector2 moveVector = Vector2.zero;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;

        InitializeComponents();
    }

    private void InitializeComponents()
    {
        // Get required components
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();

        // Initialize character managers
        InitializeCharacterManagers();
    }

    private void InitializeCharacterManagers()
    {
        // Try to find CharacterManager if not assigned
        if (characterManager == null)
        {
            characterManager = GetComponent<CharacterManager>();
        }

        // Try to find UI CharacterManager if not assigned
        if (uiCharacterManager == null)
        {
            uiCharacterManager = FindObjectOfType<CharacterManager>(true);
        }

        if (characterManager != null)
        {
            // Load the saved character index before applying character
            int selectedCharacterIndex = CharacterPersistenceManager.Instance.LoadCharacterSelection();
            characterManager.TrySelectCharacter(selectedCharacterIndex);

            // Ensure the character is properly applied
            characterManager.ApplyCharacter();
            characterManager.UpdateCharacterUI();
        }
    }

    public void Start()
    {
        ValidateComponents();

        // Setup input
        input = new CustomInput();
        input.Enable();
        input.Player.Movement.performed += OnMovementPerformed;
        input.Player.Movement.canceled += OnMovementCanceled;

        // Initialize weapons
        if (assignedWeapons.Count == 0)
        {
            AddRandomWeapon();
        }

        // Initialize UI
        if (UIController.instance != null)
        {
            UIController.instance.UpdateCoins();
        }

        // Initialize player stats
        InitializePlayerStats();

        // Play music
        if (SFXManager.instance != null)
        {
            SFXManager.instance.PlayRandomMusic();
        }
    }

    private void InitializePlayerStats()
    {
        if (PlayerStatController.instance != null)
        {
            MoveSpeed = PlayerStatController.instance.MoveSpeed[0].Value;
            PickupRange = PlayerStatController.instance.PickupRange[0].Value;
            MaxWeapons = Mathf.RoundToInt(PlayerStatController.instance.MaxWeapons[0].Value);
        }
    }

    private void ValidateComponents()
    {
        if (rb == null) Debug.LogError("Rigidbody2D missing from PlayerController");
        if (animator == null) Debug.LogError("Animator missing from PlayerController");
        if (sr == null) Debug.LogError("SpriteRenderer missing from PlayerController");
        if (characterManager == null && uiCharacterManager == null)
        {
            Debug.LogWarning("No CharacterManager assigned to PlayerController. Character switching will be disabled.");
        }
    }

    public void OnEnable()
    {
        input?.Enable();
    }

    public void OnDisable()
    {
        input?.Disable();
    }

    private void FixedUpdate()
    {
        // Apply movement
        if (rb != null)
        {
            rb.velocity = moveVector * MoveSpeed;
        }

        // Handle character flipping
        if (moveVector.x != 0)
        {
            transform.localScale = new Vector3(moveVector.x < 0 ? -1 : 1, 1, 1);
        }
    }

    private void OnMovementPerformed(InputAction.CallbackContext context)
    {
        moveVector = context.ReadValue<Vector2>();
        if (animator != null)
        {
            animator.SetBool("isRunning", moveVector.magnitude > 0);
        }
    }

    private void OnMovementCanceled(InputAction.CallbackContext context)
    {
        moveVector = Vector2.zero;
        if (animator != null)
        {
            animator.SetBool("isRunning", false);
        }
    }

    public void SwitchCharacter()
    {
        bool characterSwitched = false;

        // Try to switch character in game
        if (characterManager != null)
        {
            characterManager.NextCharacter();
            characterManager.UpdateCharacterUI();
            characterSwitched = true;
        }

        // Try to switch character in UI
        if (uiCharacterManager != null && uiCharacterManager != characterManager)
        {
            uiCharacterManager.NextCharacter();
            uiCharacterManager.UpdateCharacterUI();
            characterSwitched = true;
        }

        if (!characterSwitched)
        {
            Debug.LogError("Failed to switch character - No CharacterManager available");
        }
    }

    public void AddRandomWeapon()
    {
        if (unassignedWeapons != null && unassignedWeapons.Count > 0)
        {
            AddWeapon(unassignedWeapons[Random.Range(0, unassignedWeapons.Count)]);
        }
    }

    public void AddWeapon(Weapon weaponToAdd)
    {
        if (weaponToAdd == null) return;

        if (assignedWeapons.Count < MaxWeapons)
        {
            weaponToAdd.gameObject.SetActive(true);
            assignedWeapons.Add(weaponToAdd);
            unassignedWeapons.Remove(weaponToAdd);
        }
        else
        {
            Debug.LogWarning("Max weapons reached!");
        }
    }

    public bool IsFacingLeft()
    {
        return isFacingLeft;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("EnemyBullet"))
        {
            Destroy(gameObject);
        }
    }
}