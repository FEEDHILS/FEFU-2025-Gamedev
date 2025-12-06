using UnityEditor.SceneManagement;
using UnityEngine;

// Вешаем этот скрипт на дочерние объекты-пустышки (SnapPoints)
public class BuildSnapPoint : MonoBehaviour
{
    // Тип привязки (м.б на будущее)
    public enum SnapType { Wall, Floor, Any }
    public SnapType type = SnapType.Any;

    // Радиус срабатывания для этой конкретной точки
    public float range = 0.5f;

    void Start()
    {
        SphereCollider collider = gameObject.AddComponent<SphereCollider>();
        collider.radius = .05f;
        collider.isTrigger = true;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}