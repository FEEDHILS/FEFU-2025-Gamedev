using UnityEngine;

public class PrebuildManager : MonoBehaviour
{
    [HideInInspector] public BuildCursor Cursor;
    public Vector3 Position;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Position = Cursor.Cursor;
    }
}
