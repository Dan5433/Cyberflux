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

    [SerializeField] Transform limbs, mainCamera, cameraPivot;
    [SerializeField] float camRotationSnapSpeed = 1f;

    bool isSprinting = false;
    bool isMouseLocked = false;
    Vector2 movementInput;

    new Rigidbody rigidbody;

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();

        playerInput = new();
        playerInput.Player.Sprint.started += _ => isSprinting = true;
        playerInput.Player.Sprint.canceled += _ => isSprinting = false;
        PlayerInput.Player.MouseLock.performed += _ => isMouseLocked = !isMouseLocked;
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
        UpdateVelocity();

        if (isMouseLocked)
            limbs.rotation = Quaternion.Euler(0, mainCamera.eulerAngles.y, 0);
    }

    void UpdateVelocity()
    {
        Vector3 forwardDirection = new(mainCamera.forward.x, 0, mainCamera.forward.z);
        Vector3 rightDirection = new(mainCamera.right.x, mainCamera.right.y, 0);

        Vector3 moveDirection = forwardDirection * movementInput.y + rightDirection * movementInput.x;
        moveDirection.Normalize();

        if (moveDirection.magnitude > 0)
            RotateBodyToCamera(moveDirection);

        Vector3 velocity = moveDirection * speed;
        if (isSprinting)
            velocity *= sprintSpeedMultiplier;

        rigidbody.linearVelocity = new(velocity.x, rigidbody.linearVelocity.y, velocity.z);
    }

    void RotateBodyToCamera(Vector3 moveDirection)
    {
        moveDirection.y = 0;
        Quaternion targetRotation = Quaternion.LookRotation(moveDirection);

        limbs.rotation = Quaternion.Slerp(limbs.rotation, targetRotation, camRotationSnapSpeed);
    }

    void Update()
    {
        movementInput = playerInput.Player.Move.ReadValue<Vector2>();
        limbAnimation.UpdateAnimations(movementInput, isSprinting);
    }
}
