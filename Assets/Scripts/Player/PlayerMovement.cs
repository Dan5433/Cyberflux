using InputSystem;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    static PlayerInput playerInput;

    [SerializeField] float speed = 5f;

    new Rigidbody rigidbody;

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();

        playerInput = new();
        playerInput.Enable();
    }

    void FixedUpdate()
    {
        Vector2 movementInput = playerInput.Player.Move.ReadValue<Vector2>();

        Vector3 velocity = new(movementInput.x, rigidbody.linearVelocity.y, movementInput.y);
        velocity *= speed;
        rigidbody.linearVelocity = velocity;
    }
}
