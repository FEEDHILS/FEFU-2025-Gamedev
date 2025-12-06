// using System;
// using Codice.Client.BaseCommands;
// using UnityEngine;
// using UnityEngine.Events;
// using UnityEngine.InputSystem;

// public class BuildManager : MonoBehaviour
// {
//     [Header("Initial Settings")]
//     public Material PrebuildMaterial;
//     public GameObject Model;
//     public LayerMask Placeable;
//     public bool CanPlace = true;

//     [Header("Overlap Handling (Broken)")]
//     public bool EnableOverlapRule = true;
//     public BoxCollider OverlapTrigger;

//     [Header("Free Placement")]
//     public bool FreePlacementRule = true;
//     public bool RotateTowardsPlayer = true;
//     public bool SnapOnInput = true;

//     [Header("Grid Placement")]
//     public bool GridPlacementRule;
//     public float StepX = 1, StepY = 1, StepZ = 1;

//     [Header("GroundSnap Placement")]
//     public bool SnapToGroundRule;
//     public float SnapToGroundDistance = 3f;
//     [Tooltip("This rule will avoid snapping if collided by default. This property overwrites this behaviour")]
//     public bool SnapEvenOnCollision;

//     [Space(16)]
//     public UnityEvent OnBuildPlaced;

//     Vector3 CursorPos;
//     MeshRenderer ModelMesh;
//     Material InitialMaterial;
//     bool isPlaced = false;
//     Vector3 MoveTo;
//     void Start()
//     {
//         if (PrebuildMaterial)
//         {
//             Model.TryGetComponent<MeshRenderer>(out ModelMesh);
//             InitialMaterial = ModelMesh.material;

//             ModelMesh.material = PrebuildMaterial;
//         }
//     }

//     public void Place()
//     {
//         if (!CanPlace) return;
//         isPlaced = true;
//         print(InitialMaterial.name);
//         ModelMesh.material = InitialMaterial;

//         OnBuildPlaced?.Invoke();
//     }


//     void Update()
//     {
//         if (!isPlaced)
//             Rules();
//     }


//     void Rules()
//     {
//         CursorPos = PlayerCursor.Position;
//         bool SnapInput = InputSystem.actions.FindAction("SnapToGrid").IsPressed();


//         if (FreePlacementRule)
//             FreePlacement();

//         if (GridPlacementRule || (SnapOnInput && SnapInput))
//             GridPlacement();

//         if (SnapToGroundRule)
//             SnapToGround();

//         transform.position = MoveTo;
//     }

//     void FreePlacement()
//     {
//         MoveTo = CursorPos;

//         if (RotateTowardsPlayer)
//             transform.rotation = Quaternion.LookRotation(PlayerCursor.Anchor.forward, Model.transform.up);
//     }

//     void GridPlacement()
//     {
//         float newX = Mathf.Round(CursorPos.x / StepX) * StepX;
//         float newY = Mathf.Round(CursorPos.y / StepY) * StepY;
//         float newZ = Mathf.Round(CursorPos.z / StepZ) * StepZ;

//         MoveTo = new Vector3(newX, newY, newZ);

//         if (RotateTowardsPlayer)
//         {
//             Vector3 fwd = PlayerCursor.Anchor.forward;
//             float yaw = Mathf.Atan2(fwd.x, fwd.z) * Mathf.Rad2Deg;
            
//             float snappedYaw = Mathf.Round(yaw / 90f) * 90f;
//             transform.rotation = Quaternion.Euler(0f, snappedYaw, 0f);
//         }
            
//     }

//     void SnapToGround()
//     {
//         if (!PlayerCursor.Collided && !SnapEvenOnCollision)
//         {
//             RaycastHit hit;
//             if (Physics.Raycast(MoveTo, new Vector3(0, -1, 0), out hit, SnapToGroundDistance, Placeable))
//             {
//                 MoveTo = hit.point;
//                 print(hit.point);
//             }
//         }
//     }

//     // void CheckForOverlap()
//     // {
//     //     if (!OverlapTrigger)
//     //         Model.TryGetComponent<BoxCollider>(out OverlapTrigger);

//     //     foreach (Collider i in Physics.OverlapBox(OverlapTrigger.center, OverlapTrigger.bounds.extents, transform.rotation))
//     //     {
//     //         if (i.gameObject.TryGetComponent<BuildManager>(out BuildManager j))
//     //             CanPlace = false;
//     //     }
//     // }

//     void OnTriggerStay(Collider other)
//     {
//         if (other.TryGetComponent<BuildManager>(out BuildManager a) && EnableOverlapRule)
//         {
//             CanPlace = false;
//         }
//         else
//         {
//             CanPlace = true;
//         }
//     }
// }
