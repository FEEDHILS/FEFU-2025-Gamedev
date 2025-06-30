using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCursor : MonoBehaviour
{
    static public Vector3 Position;
    static public Transform Anchor; // Player
    public float maxDistance = 5f;

    [Tooltip("True if cursor hit something, False if cursor is in air.")]
    [SerializeField]
    static public bool Collided = false;

    void Awake()
    {
        Anchor = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.forward, out hit, maxDistance))
        {
            // Добавить логику для привязке позиции к сетке
            Position = hit.point;
            Collided = true;

            WhenHitSomething(hit);
        }
        else
        {
            Position = transform.position + transform.forward * maxDistance;
            Collided = false;
        }
    }


    void WhenHitSomething(RaycastHit hit)
    {
        bool isPressed = InputSystem.actions.FindAction("Interact").WasPressedThisFrame();
        
        if (isPressed && hit.collider.TryGetComponent<Interactable>(out Interactable a))
            a.OnUse?.Invoke();
    }
}
