using UnityEngine;

public class Camera : MonoBehaviour
{
    [SerializeField] Transform playerRoot;
    [SerializeField] float mouseSensitivity = 0.5f;
    [SerializeField] Vector2 minMaxVerticalLook = new(-90f, 90f);
    bool isMouseLocked = false;
    float zoom = 0f;

    void Update()
    {
        Vector2 lookMovement = PlayerMovement.PlayerInput.Player.Look.ReadValue<Vector2>();

        Vector3 cameraRotation = transform.rotation.eulerAngles;
        Vector3 rootRotation = playerRoot.rotation.eulerAngles;

        if (cameraRotation.x > 180f)
            cameraRotation.x -= 360f;
        cameraRotation.x -= lookMovement.y * mouseSensitivity;
        cameraRotation.x = Mathf.Clamp(cameraRotation.x, minMaxVerticalLook.x, minMaxVerticalLook.y);

        if (ShouldRotateRoot())
            rootRotation.y += lookMovement.x * mouseSensitivity;
        else
            cameraRotation.y += lookMovement.x * mouseSensitivity;


        transform.rotation = Quaternion.Euler(cameraRotation);
        playerRoot.rotation = Quaternion.Euler(rootRotation);
    }

    bool ShouldRotateRoot()
    {
        return isMouseLocked || zoom == 0;
    }
}
