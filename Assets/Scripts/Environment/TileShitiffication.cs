using UnityEngine;
using UnityEngine.UIElements;


// populate tiles with different objects)))
public class TileShitiffication : MonoBehaviour
{
    [Header("Populate Settings")]
    [Tooltip("Список префабов деревьев, камней и т.д.")]
    public Renderer Model;
    public EnvFeature[] spawnPrefabs;
    [Tooltip("Сколько объектов в среднем на плэйн")]
    public int objectsPerPlane = 10;

    void Awake()
    {
        PopulatePlane();
    }


    void PopulatePlane()
    {
        Bounds bounds = Model.bounds;

        for (int i = 0; i < objectsPerPlane; i++)
        {
            int Feature = Random.Range(0, spawnPrefabs.Length);
            if (Random.value > spawnPrefabs[Feature].Probability)
                continue;

            GameObject prefab = spawnPrefabs[Feature].Prefab;

            float px = Random.Range(bounds.min.x, bounds.max.x);
            float pz = Random.Range(bounds.min.z, bounds.max.z);
            Vector3 spawnPos = new Vector3(px, bounds.center.y, pz);

            Instantiate(prefab, spawnPos, Quaternion.Euler(Random.Range(-2, 2), Random.Range(0, 360), Random.Range(-2, 2)), gameObject.transform);
        }
    }
}
