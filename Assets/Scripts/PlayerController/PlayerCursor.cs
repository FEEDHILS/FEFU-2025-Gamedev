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
    public RaycastHit Collided;
    public RaycastHit[] CollidedTrigger;
    public LayerMask ColliderMask;

    void Awake()
    {
        Anchor = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.forward, out hit, maxDistance, ColliderMask, QueryTriggerInteraction.Ignore))
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

        // Полезно при снапинге построек
        CollidedTrigger = Physics.RaycastAll(transform.position, transform.forward, maxDistance, ColliderMask, QueryTriggerInteraction.Collide);
    }

    void WhenHitSomething()
    {
        bool isPressed = InputSystem.actions.FindAction("Interact").WasPressedThisFrame();
        
        if (UIManager.instance.CurrentState == UIManager.UIState.Closed && isPressed & Collided.collider.TryGetComponent<Interactable>(out Interactable a))
            a.OnUse?.Invoke();
    }

    void OnDrawGizmos()
    {
        if (Collided.point != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(Collided.point, 0.1f);
        }
        // if (CollidedTrigger.Length > 0 && CollidedTrigger[0].point != null)
        // {
        // Gizmos.color = Color.red;
        // Gizmos.DrawWireSphere(CollidedTrigger[0].point, 0.1f);
        // }
    }
}
