using System.Collections.Generic;
using UnityEngine;

public class TileFeatures : MonoBehaviour
{
    public BoxCollider BoundBox;
    public EnvFeature[] spawnPrefabs;
    public int objectsPerPlane = 10;
    
    [SerializeField] Vector2 Density = Vector2.one;
    [SerializeField] float NoiseScale = 0.1f;

    Bounds bounds;
    List<Vector3> points = new List<Vector3>();
    void Awake()
    {
        bounds = BoundBox.bounds;

        CalculateRandomPoints();
        PopulatePlane();
    }


    void PopulatePlane()
    {
        for (int i = 0; i < objectsPerPlane; i++)
        {
            int Feature = Random.Range(0, spawnPrefabs.Length);
            if (Random.value > spawnPrefabs[Feature].Probability)
                continue;

            GameObject prefab = spawnPrefabs[Feature].Prefab;

            int ind = Random.Range(0, points.Count);

            Instantiate(prefab, points[ind], Quaternion.Euler(Random.Range(-2, 2), Random.Range(0, 360), Random.Range(-2, 2)), gameObject.transform);
        }
    }

    void OnDrawGizmosSelected()
    {
        if (points.Count == 0)
            return;
        
        foreach(Vector3 point in points)
            Gizmos.DrawWireSphere(point, 0.1f);
    }

    [ContextMenu("Calculate Points")]
    void CalculateRandomPoints()
    {
        bounds = BoundBox.bounds;
        points.Clear();

        for (float x = bounds.min.x; x < bounds.max.x; x += Density.x)
        {
            for (float z = bounds.min.z; z < bounds.max.z; z += Density.y)
                {
                    Vector2 noise = Random.insideUnitCircle * NoiseScale;
                    
                    Vector3 point = new Vector3(x + noise.x, transform.position.y, z + noise.y);

                    if (bounds.Contains(point))
                        points.Add( point );
                }
        }
    }
}
