using System;
using UnityEngine;
using UnityEngine.Events;

public class Breakable : MonoBehaviour
{
    public DamageType BreakableType;
    public float Health = 1;
    public float WrongTypeMultiplier = 0.2f;

    public Item DropItem;
    public int Amount;

    public UnityEvent OnBreak;
    public UnityEvent OnHit;

    void Awake()
    {
        OnBreak.AddListener(() => { PlayerInventory.Instance.AddItem(DropItem, Amount); });
        OnBreak.AddListener(() => { Destroy(gameObject); });
    }

    public void TakeDamage(float Damage, DamageType DmgType)
    {
        float FinalDamage = Damage;
        OnHit?.Invoke();
        if (DmgType != BreakableType)
            FinalDamage *= WrongTypeMultiplier;

        Health -= FinalDamage;

        if (Health <= 0)
            OnBreak?.Invoke();
    }
}