using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] CharacterController controller;
    [SerializeField] float MoveSpeed = 5f;
    [SerializeField] float JumpForce = 2f;
    [SerializeField] float Gravity = 9.8f;
    [SerializeField] bool Enabled = true;
    float gravityVelocity = 0f;

    void Update()
    {
        if (Enabled)
        {
            InventoryControls();
            Movement();
        }
    }

    void FixedUpdate()
    {
        // if (Enabled)
        //     Jumping();

        controller.Move( transform.TransformDirection(gravityVelocity * Vector3.up) );
        if ((controller.collisionFlags & CollisionFlags.Below) == 0)
            gravityVelocity -= Gravity * Time.deltaTime;
        else
            gravityVelocity = 0;
    }

    void Movement()
    {
        Vector2 input = InputSystem.actions.FindAction("Move").ReadValue<Vector2>();
        Vector3 moveDirection = new Vector3(input.x, 0, input.y) * MoveSpeed * Time.deltaTime;
        controller.Move( transform.TransformDirection(moveDirection) );
    }

    void Jumping()
    {
        bool JumpInput = InputSystem.actions.FindAction("Jump").IsPressed();
        if (JumpInput && (controller.collisionFlags & CollisionFlags.Below) != 0)
            gravityVelocity += JumpForce * Time.deltaTime;
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

    public void SetEnabled(bool mode) => Enabled = mode;
}
