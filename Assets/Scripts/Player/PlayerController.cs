using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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

    private bool isFacingLeft = false; // Track player direction.



    private void Awake()
    {
        instance = this;
        input = new CustomInput();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    public void OnEnable()
    {
        input.Enable();
        input.Player.Movement.performed += OnMovementPerformed;
        input.Player.Movement.canceled += OnMovementCanceled;
    }

    public void OnDisable()
    {
        input.Disable();
        input.Player.Movement.performed -= OnMovementPerformed;
        input.Player.Movement.canceled -= OnMovementCanceled;
    }

    public void Start()
    {
        if (assignedWeapons.Count == 0)
        {
            AddRandomWeapon();
        }
        UIController.instance.UpdateCoins();

        MoveSpeed = PlayerStatController.instance.MoveSpeed[0].Value;
        PickupRange = PlayerStatController.instance.PickupRange[0].Value;
        MaxWeapons = Mathf.RoundToInt(PlayerStatController.instance.MaxWeapons[0].Value);
        SFXManager.instance.PlayRandomMusic();
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

    public Vector2 moveVector = Vector2.zero;

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

    public void AddRandomWeapon()
    {
        if (unassignedWeapons.Count > 0)
            AddWeapon(unassignedWeapons[Random.Range(0, unassignedWeapons.Count)]);
    }

    public void AddWeapon(Weapon weaponToAdd)
    {
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
}
