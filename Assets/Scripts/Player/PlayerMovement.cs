using InputSystem;
using Unity.Cinemachine;
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

    [SerializeField] CinemachineThirdPersonFollow thirdPersonFollow;
    [SerializeField] Vector3 mouseLockCamOffset;
    [SerializeField] GameObject mouseLockIcon;

    [SerializeField] float jumpStrength = 5f;

    bool isSprinting = false;
    bool isMouseLocked = false;
    bool isJumping = false;
    Vector2 movementInput;

    new Rigidbody rigidbody;

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();


        playerInput = new();

        playerInput.Player.Sprint.started += _ => isSprinting = true;
        playerInput.Player.Sprint.canceled += _ => isSprinting = false;

        playerInput.Player.MouseLock.performed += ToggleMouseLock;

        playerInput.Player.Jump.started += _ => isJumping = true;
        playerInput.Player.Jump.canceled += _ => isJumping = false;


        Cursor.lockState = CursorLockMode.Confined;
    }

    void FixedUpdate()
    {
        UpdateVelocity();
        if (isMouseLocked)
            limbs.rotation = Quaternion.Euler(0, mainCamera.eulerAngles.y, 0);

        JumpUpdate();
    }

    void Update()
    {
        movementInput = playerInput.Player.Move.ReadValue<Vector2>();
        limbAnimation.UpdateAnimations(movementInput, isSprinting);
    }

    void JumpUpdate()
    {
        const float jumpRaycastDistance = 0.05f;
        Vector3 verticalOffset = new(0, 0.025f, 0);

        if (!Physics.Raycast(transform.position + verticalOffset, Vector3.down, jumpRaycastDistance) || !isJumping)
            return;

        rigidbody.linearVelocity = new(rigidbody.linearVelocity.x, jumpStrength, rigidbody.linearVelocity.z);
    }

    void UpdateVelocity()
    {
        Vector3 forwardDirection = new(mainCamera.forward.x, 0, mainCamera.forward.z);

        Vector3 moveDirection = forwardDirection * movementInput.y + mainCamera.right * movementInput.x;
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

    void ToggleMouseLock(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        isMouseLocked = !isMouseLocked;

        mouseLockIcon.SetActive(isMouseLocked);
        Cursor.lockState = isMouseLocked ? CursorLockMode.Locked : CursorLockMode.Confined;
        thirdPersonFollow.ShoulderOffset = isMouseLocked ? mouseLockCamOffset : Vector3.zero;
    }

    public void MatchCameraRotation()
    {
        float cameraRotationY = cameraPivot.rotation.eulerAngles.y;
        transform.rotation = Quaternion.Euler(0, cameraRotationY, 0);

        limbs.localRotation = Quaternion.identity;
    }

    void OnEnable()
    {
        playerInput.Enable();
    }

    void OnDisable()
    {
        playerInput.Disable();
    }
}
