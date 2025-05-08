using UnityEngine;
using UnityEngine.InputSystem;

public class BuildPlacer : MonoBehaviour
{
    public GameObject SelectedBuilding;
    public Vector3 BuildPosition;
    public Vector3 BuildRotation;
    public BuildCursor Cursor;
    bool Availiable = true;

    void Start()
    {
        BeginBuilding(); // For Testing purposes.
    }

    // Update is called once per frame
    void Update()
    {
        // if (InputSystem.actions.FindAction("Jump").WasPressedThisDynamicUpdate() && Availiable)
        // {  
        //     Quaternion rotation = Quaternion.AngleAxis(transform.eulerAngles.y, SelectedBuilding.transform.up);

        //     GameObject build = Instantiate(SelectedBuilding, BuildPosition, rotation);
            
        // }

        
    }

    void BeginBuilding()
    {
        if (SelectedBuilding) 
        {
            GameObject prebuild = Instantiate(SelectedBuilding, BuildPosition, Quaternion.identity);
            
            PrebuildManager mg;
            if (prebuild.TryGetComponent<PrebuildManager>(out mg))
            {
                mg.Cursor = Cursor;
            }
        }
    }
}
