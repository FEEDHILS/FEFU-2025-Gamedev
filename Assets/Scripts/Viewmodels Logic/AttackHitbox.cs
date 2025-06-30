using System.Runtime.InteropServices;
using UnityEngine;


public enum DamageType
{
    Wood,
    Rock,
    Health,
}
public class AttackHitbox : MonoBehaviour
{
    [Header("Hitbox Collider")]
    public BoxCollider Box;

    [Header("Damage Settings")]
    public float Damage;
    public float ImpactForce = 1;
    public DamageType AttackType;
    void Start()
    {

    }

    // Update is called once per frame
    public void CreateHitbox()
    {
        Collider[] colliders = Physics.OverlapBox(Box.bounds.center, Box.bounds.extents, transform.rotation, Box.includeLayers);

        foreach (Collider i in colliders)
        {
            if (i.TryGetComponent<Breakable>(out Breakable breakable))
            {
                breakable.TakeDamage(Damage, AttackType);
            }

            if (i.attachedRigidbody && i.gameObject.tag != "Player")
            {
                print(i.gameObject);
                i.attachedRigidbody.AddForce((i.transform.position - transform.position).normalized * ImpactForce, ForceMode.Impulse);
            }
        }
    }
}
