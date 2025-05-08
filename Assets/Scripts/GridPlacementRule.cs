using System.Diagnostics.Tracing;
using Unity.Mathematics;
using UnityEngine;

public class GridPlacementRule : BuildRule
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float StepX = 1, StepY = 1, StepZ = 1;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float newX = Mathf.Round(mg.Position.x / StepX) * StepX;
        float newY = Mathf.Round(mg.Position.y / StepY) * StepY;
        float newZ = Mathf.Round(mg.Position.z / StepZ) * StepZ;

        transform.position = new Vector3(newX, newY, newZ);
    }
}
