using InputSystem;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    static PlayerInput playerInput;

    static readonly string ANIMATION_PARAMETER_IS_WALKING = "walking";
    static readonly string ANIMATION_PARAMETER_IS_SPRINTING = "sprinting";

    [SerializeField] float speed = 5f;
    [SerializeField] float sprintSpeedMultiplier = 1.5f;
    bool isSprinting = false;

    new Rigidbody rigidbody;
    Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
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
        Vector2 movementInput = playerInput.Player.Move.ReadValue<Vector2>();

        Vector3 velocity = new(movementInput.x, rigidbody.linearVelocity.y, movementInput.y);
        velocity *= speed;
        if (isSprinting)
            velocity *= sprintSpeedMultiplier;

        rigidbody.linearVelocity = velocity;

        UpdateAnimations(movementInput);
    }

    void UpdateAnimations(Vector2 movementInput)
    {
        animator.SetBool(ANIMATION_PARAMETER_IS_WALKING, false);
        animator.SetBool(ANIMATION_PARAMETER_IS_SPRINTING, false);

        if (movementInput.magnitude <= 0)
            return;


        if (isSprinting)
            animator.SetBool(ANIMATION_PARAMETER_IS_SPRINTING, true);
        else
            animator.SetBool(ANIMATION_PARAMETER_IS_WALKING, true);
    }
}
