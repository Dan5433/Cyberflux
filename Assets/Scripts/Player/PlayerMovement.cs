using InputSystem;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    static PlayerInput playerInput;
    public static PlayerInput PlayerInput => playerInput;

    [SerializeField] float speed = 5f;
    [SerializeField] float sprintSpeedMultiplier = 1.5f;
    [SerializeField] LimbAnimation limbAnimation;
    bool isSprinting = false;
    Vector2 movementInput;

    new Rigidbody rigidbody;

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();

        playerInput = new();
        playerInput.Player.Sprint.started += _ => isSprinting = true;
        playerInput.Player.Sprint.canceled += _ => isSprinting = false;
    }

    void OnEnable()
    {
        playerInput.Enable();
    }

    void OnDisable()
    {
        playerInput.Disable();
    }

    void FixedUpdate()
    {
        Vector3 moveDirection = (transform.forward * movementInput.y) + (transform.right * movementInput.x);
        moveDirection = moveDirection.normalized;

        moveDirection *= speed;
        if (isSprinting)
            moveDirection *= sprintSpeedMultiplier;

        rigidbody.linearVelocity = new(moveDirection.x, rigidbody.linearVelocity.y, moveDirection.z);
    }

    void Update()
    {
        movementInput = playerInput.Player.Move.ReadValue<Vector2>();
        limbAnimation.UpdateAnimations(movementInput, isSprinting);
    }
}
