using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Food", menuName = "Inventory/Create new Food Item")]
public class FoodItem : Item
{
    [Header("Food Settings")]
    public int Saturation = 1;
}