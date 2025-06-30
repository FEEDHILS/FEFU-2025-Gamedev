using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "New Feature", menuName = "Environments/Create Gen Feature")]
public class EnvFeature : ScriptableObject
{
    public GameObject Prefab;

    [Range(0f, 1f)]
    public float Probability;
}