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
    private CustomInput input;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Animator animator;
    [HideInInspector] public List<Weapon> FullyLevelledWeapons = new List<Weapon>();
    public List<Weapon> unassignedWeapons, assignedWeapons;

    public CharacterManager characterManager;

    private bool isFacingLeft = false; // Track player direction.
    public Vector2 moveVector = Vector2.zero; // Change to public or internal

    private void Awake()
    {
        // Ensure singleton pattern
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;

        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    public void Start()
    {
        input = new CustomInput();
        input.Enable();
        input.Player.Movement.performed += OnMovementPerformed;
        input.Player.Movement.canceled += OnMovementCanceled;

        if (assignedWeapons.Count == 0)
        {
            AddRandomWeapon();
        }
        UIController.instance.UpdateCoins();

        if (PlayerStatController.instance != null)
        {
            MoveSpeed = PlayerStatController.instance.MoveSpeed[0].Value;
            PickupRange = PlayerStatController.instance.PickupRange[0].Value;
            MaxWeapons = Mathf.RoundToInt(PlayerStatController.instance.MaxWeapons[0].Value);
        }

        SFXManager.instance.PlayRandomMusic();
    }

    public void OnEnable()
    {
        input?.Enable();
    }

    public void OnDisable() // Change to public or protected
    {
        input?.Disable();
    }

    private void FixedUpdate()
    {
        rb.velocity = moveVector * MoveSpeed;

        // Flip the character based on movement direction
        if (moveVector.x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (moveVector.x > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
    }

    private void OnMovementPerformed(InputAction.CallbackContext context)
    {
        moveVector = context.ReadValue<Vector2>();
        animator.SetBool("isRunning", moveVector.magnitude > 0);
    }

    private void OnMovementCanceled(InputAction.CallbackContext context)
    {
        moveVector = Vector2.zero;
        animator.SetBool("isRunning", false);
    }

    public void SwitchCharacter()
    {
        characterManager.NextCharacter(); // Call the NextCharacter method from CharacterManager
        characterManager.UpdateCharacterUI(); // Update the UI image
        // UIController.instance.Settings();
        // UIController.instance.PauseUnpause();
    }

    public void AddRandomWeapon()
    {
        if (unassignedWeapons.Count > 0)
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

    // Method to update player direction based on input
    public void UpdatePlayerDirection(float horizontalInput)
    {
        if (horizontalInput < 0)
        {
            isFacingLeft = true;
        }
        else if (horizontalInput > 0)
        {
            isFacingLeft = false;
        }
    }

    // Method to check if player is facing left
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
