using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCursor : MonoBehaviour
{
    public static PlayerCursor instance = null;
    void OnEnable() => instance = this;
    void OnDisable() => instance = null;
    public Vector3 Position;
    public Transform Anchor;
    public float maxDistance = 5f;

    [Tooltip("True if cursor hit something, False if cursor is in air.")]
    static public RaycastHit Collided;
    public LayerMask ColliderMask;

    void Awake()
    {
        Anchor = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.forward, out hit, maxDistance, ColliderMask))
        {
            // Добавить логику для привязке позиции к сетке
            Position = hit.point;
            Collided = hit;

            WhenHitSomething();
        }
        else
        {
            Position = transform.position + transform.forward * maxDistance;
            Collided = new RaycastHit(); 
            hit.point = Position;
        }
    }


    void WhenHitSomething()
    {
        bool isPressed = InputSystem.actions.FindAction("Interact").WasPressedThisFrame();
        
        if (isPressed & Collided.collider.TryGetComponent<Interactable>(out Interactable a))
            a.OnUse?.Invoke();
    }
}
