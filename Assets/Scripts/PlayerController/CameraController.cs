using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    Vector3 CurrentRotation = new Vector3(90, 0, 0);
    public float Sensitivity = 0.2f;
    //public float FollowSpeed = 5f;
    public GameObject Player; // Вращаем игрок если он есть
    bool Focused = false;

    public UnityEvent OnFocused;
    public UnityEvent OnUnfocused;

    void Start()
    {
        MouseLock();
    }


    void Update()
    {
        // if (InputSystem.actions.FindAction("Click").IsPressed() && !Focused)
        //     MouseLock();

        if (Cursor.lockState == CursorLockMode.None) return;

        Vector3 mouseInput = InputSystem.actions.FindAction("Look").ReadValue<Vector2>() * (Sensitivity / 100);
        CurrentRotation.x -= mouseInput.y;
        CurrentRotation.x = Mathf.Clamp(CurrentRotation.x, 0, 180);

        CurrentRotation.y += mouseInput.x;

        transform.localRotation = Quaternion.Euler(CurrentRotation.x - 90, 0, 0);

        Player.GetComponent<Rigidbody>().MoveRotation(Quaternion.Euler(0, CurrentRotation.y, 0));

        if (InputSystem.actions.FindAction("Cancel").IsPressed())
            MouseUnlock();
    }

    public void MouseLock()
    {
        // CurrentRotation = transform.rotation.eulerAngles;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Focused = true;

        OnFocused?.Invoke();
    }
    
    public void MouseUnlock()
    {
        // CurrentRotation = transform.rotation.eulerAngles;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Focused = false;

        OnUnfocused?.Invoke();
    }
}
