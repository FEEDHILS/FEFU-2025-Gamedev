using System;
using System.Buffers.Text;
using UnityEngine;

[CreateAssetMenu(fileName = "New Build", menuName = "Inventory/Create new Build Item")]
public class BuildItem : Item
{
    [Header("Buildings Settings")]
    public GameObject Prebuild;
}