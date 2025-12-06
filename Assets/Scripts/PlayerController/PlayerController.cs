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
        Movement();
        InventoryControls();
    }

    void Movement()
    {
        Vector2 input = InputSystem.actions.FindAction("Move").ReadValue<Vector2>();
        Vector3 moveDirection = new Vector3(input.x, 0, input.y) * MoveSpeed;

        Vector3 moveVector = rb.transform.right * moveDirection.x + rb.transform.forward * moveDirection.z;
        rb.MovePosition(rb.position + moveVector * Time.deltaTime);
    }

    void InventoryControls()
    {
        PlayerInventory inv = PlayerInventory.Instance;
        
        bool modifier = InputSystem.actions.FindAction("DropAll").IsPressed();
        if (InputSystem.actions.FindAction("Drop").WasPressedThisFrame())
        {
            inv.RemoveAt(inv.SelectedSlot, modifier ? inv.SelectedSlot.count : 1);
        }

        
        if (InputSystem.actions.FindAction("Hotbar1").WasPressedThisFrame())
            inv.ChangeSelected(0);

        if (InputSystem.actions.FindAction("Hotbar2").WasPressedThisFrame())
            inv.ChangeSelected(1);

        if (InputSystem.actions.FindAction("Hotbar3").WasPressedThisFrame())
            inv.ChangeSelected(2);
            
        if (InputSystem.actions.FindAction("Hotbar4").WasPressedThisFrame())
            inv.ChangeSelected(3);
    }
}
