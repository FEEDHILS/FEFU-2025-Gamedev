using UnityEngine;
using UnityEngine.InputSystem;

public class BuildPlacer : MonoBehaviour
{
    public GameObject SelectedBuilding;
    public Vector3 BuildPosition;
    public Vector3 BuildRotation;
    public bool Availiable = false;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (InputSystem.actions.FindAction("Jump").WasPressedThisDynamicUpdate() && Availiable)
        {  
            Quaternion rotation = Quaternion.AngleAxis(transform.eulerAngles.y, SelectedBuilding.transform.up);

            GameObject build = Instantiate(SelectedBuilding, BuildPosition, rotation);
            
        }
    }
}
