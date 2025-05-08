using UnityEngine;

public class BuildCursor : MonoBehaviour
{
    public Vector3 Cursor;
    public float maxDistance = 5f;
    public BuildPlacer Placer;
    void Start()
    {

    }

    void Update()
    {
        RaycastHit hit;

        if ( Physics.Raycast(transform.position, transform.forward, out hit, maxDistance) )
        {
            // Добавить логику для привязке позиции к сетке
            Cursor = hit.point;

            // Placer.Availiable = true;
            Placer.BuildPosition = Cursor;
        }
        else
            Cursor = transform.position + transform.forward * maxDistance;
            // Placer.Availiable = false;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward * maxDistance);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(Cursor, 0.5f);
    }
}
