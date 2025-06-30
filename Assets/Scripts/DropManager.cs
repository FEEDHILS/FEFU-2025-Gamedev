using System.Collections.Generic;
using UnityEngine;

public class DropManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject DropPrefab;
    public static DropManager Instance;

    List<DropItem> AllWorldDrops = new List<DropItem>();
    void Awake()
    {
        Instance = this;
    }


    public void Drop(Item item, int amount, Vector3 At, Quaternion Rotation, int Force=0)
    {
        GameObject drop = Instantiate(DropPrefab, At, Rotation);
        DropItem component = drop.GetComponent<DropItem>();
        component.Item = item;
        component.Amount = amount;

        drop.GetComponent<Rigidbody>().AddForce(( drop.transform.forward + Vector3.up).normalized * Force, ForceMode.Impulse);

        AllWorldDrops.Add(component);
    }
}
