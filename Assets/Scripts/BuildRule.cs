using UnityEngine;

public class BuildRule : MonoBehaviour
{
    [HideInInspector] public PrebuildManager mg;
    void Awake()
    {
        mg = GetComponent<PrebuildManager>();
    }
}
