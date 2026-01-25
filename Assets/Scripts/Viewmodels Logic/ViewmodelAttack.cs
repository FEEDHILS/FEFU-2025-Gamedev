using System.Runtime.InteropServices;
using UnityEngine;

public class ViewmodelAttack : MonoBehaviour
{
    public enum WeaponTypes
    {
        Axe,
        Pickaxe,
        Sword,
    }

    [SerializeField] float Range = 3f;
    public float Damage;
    public float ImpactForce = 1;
    public WeaponTypes WeaponType;
    void Start()
    {

    }


    public void Action()
    {
        RaycastHit hit = PlayerCursor.instance.Collided;
        
        if ((hit.point - PlayerCursor.instance.Anchor.position).magnitude > Range)
            return;

        Collider i = hit.collider;
        if (i.TryGetComponent<Breakable>(out Breakable breakable))
        {
            breakable.TakeDamage(Damage, WeaponType, hit);
        }

        if (i.attachedRigidbody && i.gameObject.tag != "Player")
        {
            print(i.gameObject);
            i.attachedRigidbody.AddForce((i.transform.position - transform.position).normalized * ImpactForce, ForceMode.Impulse);
        }
    }
}
