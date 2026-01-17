using NUnit.Framework.Internal;
using UnityEngine;


public class BuildSnapPoint : MonoBehaviour
{
    public enum Orientations {Horizontal, Vertical}
    public enum SnapTypes { Wall, Floor, Any }
    public Orientations Orientation = Orientations.Horizontal;
    public SnapTypes SnapType = SnapTypes.Any;
    public float RotationSnap = 90; // Понадобится в SnapPlacement компоненте.

    public float range = 0.5f;
    public Vector3 PointDirection;
    public float testAngle;
    [ContextMenu("Начальная конфигурация")]
    void Start()
    {
        SphereCollider collider = gameObject.AddComponent<SphereCollider>();
        collider.radius = range;
        collider.isTrigger = true;
        PointDirection = (transform.position - transform.parent.position).normalized;

        testAngle = Vector2.SignedAngle(new Vector2(transform.parent.forward.x,transform.parent.forward.z), new Vector2(PointDirection.x, PointDirection.z));
    }

    void Update()
    {
        PointDirection = (transform.position - transform.parent.position).normalized;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, range);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + PointDirection);
    }
}