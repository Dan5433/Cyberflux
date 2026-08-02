using UnityEngine;

public class CameraControls : MonoBehaviour
{
    [SerializeField] Transform cameraPivot, playerRoot, head;
    [SerializeField] float mouseSensitivity = 0.5f;
    [SerializeField] float zoomSensitivity = 0.25f;
    [SerializeField] Vector2 minMaxVerticalLook = new(-90f, 90f);
    [SerializeField] float maxZoomOut = 10f;
    bool isMouseLocked = false;
    float zoomOut = 0f;

    public Transform CameraPivot => cameraPivot;

    void Update()
    {
        UpdateZoom();
        UpdateRotation();

        if (zoomOut < 0)
            transform.LookAt(head);
    }

    void UpdateRotation()
    {
        Vector2 lookMovement = PlayerMovement.PlayerInput.Player.Look.ReadValue<Vector2>();

        Vector3 rootRotation = playerRoot.rotation.eulerAngles;
        Vector3 cameraRotation = cameraPivot.localRotation.eulerAngles;

        if (cameraRotation.x > 180f)
            cameraRotation.x -= 360f;
        cameraRotation.x -= lookMovement.y * mouseSensitivity;
        cameraRotation.x = Mathf.Clamp(cameraRotation.x, minMaxVerticalLook.x, minMaxVerticalLook.y);


        if (ShouldRotateRoot())
        {
            rootRotation.y += lookMovement.x * mouseSensitivity;
            cameraRotation.y = 0;
        }
        else
            cameraRotation.y += lookMovement.x * mouseSensitivity;


        playerRoot.rotation = Quaternion.Euler(rootRotation);
        cameraPivot.localRotation = Quaternion.Euler(cameraRotation);
    }

    void UpdateZoom()
    {
        Vector2 scrollInput = PlayerMovement.PlayerInput.Player.Zoom.ReadValue<Vector2>();

        zoomOut -= scrollInput.y * zoomSensitivity;
        zoomOut = Mathf.Clamp(zoomOut, 0, maxZoomOut);

        Vector3 desiredPosition = cameraPivot.position - cameraPivot.forward * zoomOut;
        transform.position = desiredPosition;
    }

    bool ShouldRotateRoot()
    {
        return isMouseLocked || zoomOut == 0;
    }
}
