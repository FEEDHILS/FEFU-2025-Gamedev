using System;
using System.Buffers.Text;
using UnityEngine;

[CreateAssetMenu(fileName = "New Tool", menuName = "Inventory/Create new Tool Item")]
public class ToolItem : Item
{
    [Header("Tool Settings")]
    public GameObject Viewmodel;

}