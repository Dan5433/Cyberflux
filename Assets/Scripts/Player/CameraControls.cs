using Unity.Cinemachine;
using UnityEngine;

public class CameraControls : MonoBehaviour
{
    [SerializeField] CinemachineThirdPersonFollow thirdPersonFollow;
    [SerializeField] Transform playerRoot;
    [SerializeField] float mouseSensitivity = 0.5f;
    [SerializeField] float zoomSensitivity = 0.25f;
    [SerializeField] Vector2 minMaxVerticalLook = new(-90f, 90f);
    [SerializeField] float maxCameraDistance = 10f;
    bool isMouseLocked = false;
    float cameraDistance = 0f;

    void Update()
    {
        UpdateZoom();
        UpdateRotation();
    }

    void UpdateRotation()
    {
        Vector2 lookMovement = PlayerMovement.PlayerInput.Player.Look.ReadValue<Vector2>();

        Vector3 rootRotation = playerRoot.rotation.eulerAngles;
        Vector3 cameraRotation = transform.localRotation.eulerAngles;

        if (cameraRotation.x > 180f)
            cameraRotation.x -= 360f;
        cameraRotation.x -= lookMovement.y * mouseSensitivity;
        cameraRotation.x = Mathf.Clamp(cameraRotation.x, minMaxVerticalLook.x, minMaxVerticalLook.y);


        if (ShouldRotateRoot())
        {
            rootRotation.y += lookMovement.x * mouseSensitivity;
            cameraRotation.y = 0;
        }
        cameraRotation.y += lookMovement.x * mouseSensitivity;


        playerRoot.rotation = Quaternion.Euler(rootRotation);
        transform.localRotation = Quaternion.Euler(cameraRotation);
    }

    void UpdateZoom()
    {
        Vector2 scrollInput = PlayerMovement.PlayerInput.Player.Zoom.ReadValue<Vector2>();

        cameraDistance -= scrollInput.y * zoomSensitivity;
        cameraDistance = Mathf.Clamp(cameraDistance, 0, maxCameraDistance);

        thirdPersonFollow.CameraDistance = cameraDistance;
    }

    bool ShouldRotateRoot()
    {
        return isMouseLocked || cameraDistance == 0;
    }
}
