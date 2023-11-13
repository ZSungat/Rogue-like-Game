using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement instance;
    public float MovementSpeed = 4f;
    public float PickupRange = 1.5f;

    private CustomInput input;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Animator animator;
    //public Weapon ActiveWeapon;
    public List<Weapon> unassignedWeapons, assignedWeapons;

    public Vector2 moveVector = Vector2.zero;

    private void Awake()
    {
        instance = this;
        input = new CustomInput();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }
    private void Start()
    {
        if (assignedWeapons.Count == 0)
        {
            AddWeapon(Random.Range(0, unassignedWeapons.Count));
        }
    }
    private void OnEnable()
    {
        input.Enable();
        input.Player.Movement.performed += OnMovementPerformed;
        input.Player.Movement.canceled += onMovementCanceled;
    }
    private void OnDisable()
    {
        input.Disable();
        input.Player.Movement.performed -= OnMovementPerformed;
        input.Player.Movement.canceled -= onMovementCanceled;
    }

    private void FixedUpdate()
    {
        rb.velocity = moveVector * MovementSpeed;
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
