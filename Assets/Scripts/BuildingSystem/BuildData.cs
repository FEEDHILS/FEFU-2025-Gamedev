using UnityEngine;

[CreateAssetMenu(fileName = "New Build", menuName = "Buildings/Register Basic Build")]
public class BuildData : ScriptableObject
{
    public Mesh SchematicModel;
    public Vector3 OriginOffset = Vector3.zero; // Если центр модели нужно подвинуть
    public GameObject BuildPrefab;
}

[CreateAssetMenu(fileName = "New Build", menuName = "Buildings/Register Complex Build")]
public class CBuildData : BuildData
{
    public GameObject SchematicPrefab;
}