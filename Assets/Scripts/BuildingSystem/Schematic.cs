using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Schematic : MonoBehaviour
{
    public BuildItem ItemRef;
    public GameObject Build;
    public Material Available;
    public Material Unavailable;
    public bool DisableSchematic = false;

    [Header("Настройки передвижения")]
    public bool FreeAvailiable = true; // Разрешено ли свободное перемещение (которое просто за курсором следует)
    public bool GridAvailiable = true;

    public Vector3 GridMetrics = new Vector3(0.5f, 0.5f, 0.5f);
    public float RayToGroundDistance = 5f; // Для привязки к земле

    public bool DisableGroundClipping = false;
    public BoxCollider box;

    [Header("Настройки Поворота")]
    public bool AllowManualRotation = true;
    public float RotationSnap = 5f;

    public UnityEvent OnAction; 
    public UnityEvent OnPlaced; 

    bool GridMove;
    bool ToGround;
    Bounds modelBounds;
    bool ManualRotation = false; // off by default
    float cumulativeRotation = 0f;
    Material[] initialMat;
    float boundsCenterOffset;

    private List<(MeshRenderer renderer, Material material)> initialSingleMaterials = new List<(MeshRenderer, Material)>();
    void Awake()
    {
        Build.TryGetComponent(out Collider col);
        if (box || TryGetComponent(out box))
        {
            modelBounds = box.bounds;
        }
        else if (Build.TryGetComponent(out MeshRenderer renderer))
        {
            modelBounds = renderer.bounds;
        }
        else
        {
            Debug.LogWarning("No BoxCollider for defining Schematic shape", this);
            modelBounds = new Bounds(transform.position, Vector3.one * 0.05f);
        }
        col.enabled = false;
        boundsCenterOffset = (modelBounds.center - transform.position).y;
        
        foreach (MeshRenderer renderer in Build.GetComponentsInChildren<MeshRenderer>())
        {
            if (renderer.sharedMaterials.Length > 0)
            {
                // Сохраняем только первый материал
                initialSingleMaterials.Add((renderer, renderer.sharedMaterial));
                
                // Меняем только первый материал на Available
                Material[] newMats = renderer.sharedMaterials;
                newMats[0] = Available;
                renderer.sharedMaterials = newMats;
            }
        }

        if (TryGetComponent(out Interactable i))
            i.enabled = false;

        if (DisableSchematic)
            Place();
    }

    public void Place()
    {
        foreach (var (renderer, originalMat) in initialSingleMaterials)
        {
            if (renderer != null && renderer.sharedMaterials.Length > 0)
            {
                Material[] newMats = renderer.sharedMaterials;
                newMats[0] = originalMat;
                renderer.sharedMaterials = newMats;
            }
        }

        Build.TryGetComponent(out Collider col);
        col.enabled = true;
        if (TryGetComponent(out Interactable i))
            i.enabled = true;

        DisableSchema();
        if (!Build.TryGetComponent(out Breakable _))
        {
            CreateBlankBreakable();
        }
        OnPlaced?.Invoke();
    }

    [ContextMenu("Add Breakable to Build")]
    public void CreateBlankBreakable()
    {
        Breakable comp = Build.AddComponent<Breakable>();
        comp.Amount = 1;
        comp.Health = 10;
        comp.DropItem = ItemRef;
        comp.WrongTypeMultiplier = 1;
        comp.Regenerate = true;
    }

    // Вместо обычной постройки оставляет схему, которую можно потом достроить если надо
    public void DisableSchema()
    {
        DisableSchematic = true;

        if (TryGetComponent(out SnapPlacement Snapper))
            Snapper.enabled = false;
    }

    void Update()
    {
        if (DisableSchematic) 
            return;

        InputHandle();
        RotationBehaviors();
        PositionBehaviors();
    }
    
    // TODO: Не думаю что этому классу стоит управлять еще и этим, лучше вынести в Builder.
    void InputHandle()
    {
        GridMove = InputSystem.actions.FindAction("SnapToGrid").IsPressed();
        if (!FreeAvailiable)
            GridMove = true;

        ToGround = InputSystem.actions.FindAction("SnapToGround").IsPressed();
        
        cumulativeRotation += InputSystem.actions.FindAction("RotateBuild").ReadValue<float>();
        
        if (InputSystem.actions.FindAction("RotateBuild").inProgress && AllowManualRotation && !ManualRotation)
            ManualRotation = true;
        
        if (InputSystem.actions.FindAction("RotateToPlayer").inProgress && ManualRotation)
            ManualRotation = false;
    }

    void RotationBehaviors()
    {
        transform.rotation = Quaternion.LookRotation(PlayerCursor.instance.Anchor.forward, transform.up);

        if (ManualRotation)
        {
            transform.rotation = Quaternion.AngleAxis(cumulativeRotation*RotationSnap, transform.up);
        }
        else if(GridMove)
        {
            Vector3 fwd = PlayerCursor.instance.Anchor.forward;
            float yaw = Mathf.Atan2(fwd.x, fwd.z) * Mathf.Rad2Deg;
            
            float snappedYaw = Mathf.Round(yaw / 90f) * 90f;
            transform.rotation = Quaternion.Euler(0f, snappedYaw, 0f);
        }
    }

    void PositionBehaviors()
    {
        // Свободное (Стандартное) перемещение
        if (FreeAvailiable)
            transform.position = PlayerCursor.instance.Position;

        if (GridMove && GridAvailiable)
        {
            Vector3 CursorPos = PlayerCursor.instance.Position;
            Vector3 Grid = GridMetrics;

            float newX = Mathf.Round(CursorPos.x / Grid.x) * Grid.x;
            float newY = Mathf.Round(CursorPos.y / Grid.y) * Grid.y;
            float newZ = Mathf.Round(CursorPos.z / Grid.z) * Grid.z;

            transform.position = new Vector3(newX, newY, newZ);  
        }

        if (ToGround)
        {
            RaycastHit hit;
            if (Physics.SphereCast(PlayerCursor.instance.Position + new Vector3(0, modelBounds.extents.y, 0), 0.1f, new Vector3(0, -1, 0), out hit, RayToGroundDistance, Physics.AllLayers, QueryTriggerInteraction.Ignore))
            {
                transform.position = new Vector3(transform.position.x, hit.point.y, transform.position.z);
            }
        }

        if (DisableGroundClipping)
            KeepAboveGround();
    }


    public void KeepAboveGround()
    {
        Vector3 RayPos = PlayerCursor.instance.Position + Vector3.up;
        RaycastHit hit;
        if (Physics.SphereCast(RayPos, 0.1f, Vector3.down, out hit, 4*modelBounds.extents.y, Physics.AllLayers, QueryTriggerInteraction.Ignore))
        {
            Vector3 clampedPos = new Vector3(transform.position.x, hit.point.y + modelBounds.extents.y - boundsCenterOffset, transform.position.z);
            transform.position = Vector3.Max(transform.position, clampedPos);
        }
    }
}
