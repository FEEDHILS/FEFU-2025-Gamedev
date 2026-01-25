using System.Collections.Generic;
using UnityEngine;

public class DropManager : MonoBehaviour
{
    public GameObject DropPrefab;
    public static DropManager Instance;
    [SerializeField] float Force = 1f;

    List<DropItem> AllWorldDrops = new List<DropItem>();
    void Awake()
    {
        Instance = this;
    }

    public void Drop(Item item, int amount, Vector3 At, Quaternion Rotation)
    {
        GameObject drop = Instantiate(DropPrefab, At, Rotation);
        DropItem component = drop.GetComponent<DropItem>();
        component.Item = item;
        component.Amount = amount;

        Vector2 random = Random.insideUnitCircle;
        drop.GetComponent<Rigidbody>().AddForce(new Vector3(random.x, 1, random.y).normalized * Force, ForceMode.Impulse);

        AllWorldDrops.Add(component);
    }

    public void Drop(Item item, int amount, Vector3 At, Quaternion Rotation, Vector3 ForceVector)
    {
        GameObject drop = Instantiate(DropPrefab, At, Rotation);
        DropItem component = drop.GetComponent<DropItem>();
        component.Item = item;
        component.Amount = amount;

        drop.GetComponent<Rigidbody>().AddForce(ForceVector * Force, ForceMode.Impulse);
        AllWorldDrops.Add(component);
    }
}
