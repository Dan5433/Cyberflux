using UnityEngine;

public class Camera : MonoBehaviour
{
    [SerializeField] Transform playerRoot;
    [SerializeField] float mouseSensitivity = 0.5f;
    bool isMouseLocked = false;
    float zoom = 0f;

    void Update()
    {
        Vector2 lookMovement = PlayerMovement.PlayerInput.Player.Look.ReadValue<Vector2>();

        Vector3 rotation;
        if (isMouseLocked || zoom == 0)
            rotation = playerRoot.rotation.eulerAngles;
        else
            rotation = transform.rotation.eulerAngles;

        rotation.x -= lookMovement.y * mouseSensitivity;
        rotation.y += lookMovement.x * mouseSensitivity;
        rotation.z = 0;

        if (isMouseLocked || zoom == 0)
            playerRoot.rotation = Quaternion.Euler(rotation);
        else
            transform.rotation = Quaternion.Euler(rotation);
    }
}
