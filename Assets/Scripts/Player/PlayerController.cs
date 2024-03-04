using System.Collections;
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

    public Vector2 moveVector = Vector2.zero;

    private void Awake()
    {
        instance = this;
        input = new CustomInput();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();

        Application.targetFrameRate = 144;
    }
    void Start()
    {
        if (assignedWeapons.Count == 0)
        {
            AddWeapon(Random.Range(0, unassignedWeapons.Count));
        }
        UIController.instance.UpdateCoins();

        MoveSpeed = PlayerStatController.instance.MoveSpeed[0].Value;
        PickupRange = PlayerStatController.instance.PickupRange[0].Value;
        MaxWeapons = Mathf.RoundToInt(PlayerStatController.instance.MaxWeapons[0].Value);
    }
    public void OnEnable()
    {
        input.Enable();
        input.Player.Movement.performed += OnMovementPerformed;
        input.Player.Movement.canceled += onMovementCanceled;
    }
    public void OnDisable()
    {
        input.Disable();
        input.Player.Movement.performed -= OnMovementPerformed;
        input.Player.Movement.canceled -= onMovementCanceled;
    }

    private void FixedUpdate()
    {
        rb.velocity = moveVector * MoveSpeed;
    }
    private void OnMovementPerformed(InputAction.CallbackContext CallbackContext)
    {
        moveVector = CallbackContext.ReadValue<Vector2>();
        if (moveVector.x > 0)
        {
            sr.flipX = false;
            // transform.localScale = Vector3.one;
        }
        else
        {
            sr.flipX = true;
            // transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        animator.SetBool("isRunning", true);
    }
    private void onMovementCanceled(InputAction.CallbackContext CallbackContext)
    {
        moveVector = Vector2.zero;
        animator.SetBool("isRunning", false);
    }

    // public void AddWeapon(int WeaponNumber)
    // {
    //     if (WeaponNumber < unassignedWeapons.Count)
    //     {
    //         assignedWeapons.Add(unassignedWeapons[WeaponNumber]);

    //         unassignedWeapons[WeaponNumber].gameObject.SetActive(true);
    //         unassignedWeapons.RemoveAt(WeaponNumber);
    //     }
    // }
    public void AddWeapon(int WeaponNumber)
    {
        if (WeaponNumber < unassignedWeapons.Count)
        {
            assignedWeapons.Add(unassignedWeapons[WeaponNumber]);

            unassignedWeapons[WeaponNumber].gameObject.SetActive(true);
            unassignedWeapons.RemoveAt(WeaponNumber);
        }
    }

    public void AddWeapon(Weapon WeaponToAdd)
    {
        WeaponToAdd.gameObject.SetActive(true);

        assignedWeapons.Add(WeaponToAdd);
        unassignedWeapons.Remove(WeaponToAdd);
    }
}

