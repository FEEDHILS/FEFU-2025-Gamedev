using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    Rigidbody rb;
    public float MoveSpeed = 5f;
    public float MovementDamp;

    Vector3 currentDirection;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        Vector2 input = InputSystem.actions.FindAction("Move").ReadValue<Vector2>();

        Vector3 moveDirection = new Vector3(input.x, 0, input.y) * MoveSpeed;

        // currentDirection = Vector3.Lerp(currentDirection, moveDirection, MovementDamp * Time.deltaTime);

        Vector3 moveVector = rb.transform.right * moveDirection.x + rb.transform.forward * moveDirection.z;

        // print(moveVector);
        rb.MovePosition(rb.position + moveVector * Time.deltaTime );

        // rb.linearVelocity = currentDirection;
    }
}
