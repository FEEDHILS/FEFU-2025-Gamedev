using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    Vector3 CurrentRotation;
    public float Sensitivity = 0.2f;
    //public float FollowSpeed = 5f;
    public GameObject Player; // Вращаем игрок если он есть
    bool Focused = false;

    void Start()
    {

    }


    void Update()
    {
        if (InputSystem.actions.FindAction("Click").IsPressed() && !Focused)
            MouseLock();

        if (!Focused)
            return;

        Vector3 mouseInput = InputSystem.actions.FindAction("Look").ReadValue<Vector2>() * (Sensitivity / 100);
        CurrentRotation.x -= mouseInput.y;
        CurrentRotation.y += mouseInput.x;
        CurrentRotation.x = Mathf.Clamp( CurrentRotation.x, -90, 90 );

        transform.localRotation = Quaternion.Euler(CurrentRotation.x, 0, 0);
        Player.GetComponent<Rigidbody>().MoveRotation( Quaternion.Euler(0, CurrentRotation.y, 0) );

        if (InputSystem.actions.FindAction("Cancel").IsPressed())
            Focused = false;
    }

    void MouseLock()
    {
        CurrentRotation = transform.rotation.eulerAngles;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Focused = true;
    }
}
