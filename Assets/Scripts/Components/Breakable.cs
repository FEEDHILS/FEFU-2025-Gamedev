using System;
using UnityEngine;
using UnityEngine.Events;

public class Breakable : MonoBehaviour
{
    public MaterialTypes Material;
    public float Health = 1;
    public float WrongTypeMultiplier = 0.2f;

    public Item DropItem;
    public int Amount;

    bool OnBreakOverride = false; // Перезаписывает стандартное поведение при ломании.
    public UnityEvent OnBreak;
    public UnityEvent OnHit;

    void Awake()
    {
        if (!OnBreakOverride)
            OnBreak.AddListener(() => { PlayerInventory.Instance.AddItem(DropItem, Amount); Destroy(gameObject); });
    }

    public void TakeDamage(float Damage, ViewmodelAttack.WeaponTypes Weapon)
    {
        float FinalDamage = Damage;
        OnHit?.Invoke();
        if (!IsSuitableWeapon(Weapon))
            FinalDamage *= WrongTypeMultiplier;

        Health -= FinalDamage;

        if (Health <= 0)
            OnBreak?.Invoke();
    }

    bool IsSuitableWeapon(ViewmodelAttack.WeaponTypes Weapon)
    {
        switch(Material)
        {
            case MaterialTypes.Wood:
                if (Weapon == ViewmodelAttack.WeaponTypes.Axe)
                    return true;
                else
                    return false;


            case MaterialTypes.Stone:
                if (Weapon == ViewmodelAttack.WeaponTypes.Pickaxe)
                    return true;
                else
                    return false;
        }

        Debug.LogWarning("Unkown MaterialType", this);
        return false;
    }
}