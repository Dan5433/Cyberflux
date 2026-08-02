using UnityEngine;

public class CameraControls : MonoBehaviour
{
    [SerializeField] Transform cameraPivot, playerRoot, head;
    [SerializeField] float mouseSensitivity = 0.5f;
    [SerializeField] float zoomSensitivity = 0.25f;
    [SerializeField] Vector2 minMaxVerticalLook = new(-90f, 90f);
    [SerializeField] float maxZoomOut = 10f;
    bool isMouseLocked = false;
    float zoom = 0f;

    void Update()
    {
        UpdateZoom();
        UpdateRotation();

        if (zoom < 0)
            transform.LookAt(head);
        else
            transform.localRotation = Quaternion.identity;
    }

    void UpdateRotation()
    {
        Vector2 lookMovement = PlayerMovement.PlayerInput.Player.Look.ReadValue<Vector2>();

        Vector3 rootRotation = playerRoot.rotation.eulerAngles;
        Vector3 cameraRotation = cameraPivot.rotation.eulerAngles;

        if (cameraRotation.x > 180f)
            cameraRotation.x -= 360f;
        cameraRotation.x -= lookMovement.y * mouseSensitivity;
        cameraRotation.x = Mathf.Clamp(cameraRotation.x, minMaxVerticalLook.x, minMaxVerticalLook.y);


        if (ShouldRotateRoot())
            rootRotation.y += lookMovement.x * mouseSensitivity;
        else
            cameraRotation.y += lookMovement.x * mouseSensitivity;


        cameraPivot.rotation = Quaternion.Euler(cameraRotation);
        playerRoot.rotation = Quaternion.Euler(rootRotation);
    }

    void UpdateZoom()
    {
        Vector2 scrollInput = PlayerMovement.PlayerInput.Player.Zoom.ReadValue<Vector2>();

        zoom += scrollInput.y * zoomSensitivity;
        zoom = Mathf.Clamp(zoom, -maxZoomOut, 0);

        Vector3 desiredPosition = cameraPivot.position - cameraPivot.forward * zoom;
        transform.position = desiredPosition;
    }

    bool ShouldRotateRoot()
    {
        return isMouseLocked || zoom == 0;
    }
}
