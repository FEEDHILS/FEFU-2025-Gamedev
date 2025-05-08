using System.Security.Cryptography;
using UnityEngine;

public class FreePlacementRule : BuildRule
{
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = mg.Position;
    }
}
