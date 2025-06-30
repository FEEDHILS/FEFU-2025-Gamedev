using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Create new Basic Item")]
public class Item : ScriptableObject
{
    [Header("Initial Data")]
    public string itemName = "";
    public int maxStackSize = 1;

    [Header("Appearance and Visuals")]
    public Sprite icon;
    public GameObject DropModel = null;


    // maybe usefull
    public virtual void Use()
    {
    }
    public virtual void OnEquip()
    {

    }
    // public virtual void OnUnequip();
}