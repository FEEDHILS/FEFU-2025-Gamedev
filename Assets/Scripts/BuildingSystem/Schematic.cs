using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Schematic : MonoBehaviour
{
    public UnityEvent OnAction; 

    public GameObject Build;
    [Header("Материалы")]
    public Material Available;
    public Material Unavailable;


    [Header("Настройки перемещения")]
    public bool LockSchematic = false; 

    bool GridMove;
    public Vector3 Grid = new Vector3(0.5f, 0.5f, 0.5f); // "Размеры сетки привязки"

    bool ToGround;
    float GroundDistance = 5f; // Макс. расстояние на котором действует привязка к полу
    
    public bool DisableGroundClipping = true; // Не дает модели пройти сквозь землю
    Bounds modelBounds; // Нужно чтоб модель не проваливалась под карту


    [Header("Настройки Привязки")]
    public Transform SnapPointsParent; 
    private List<Transform> mySnapPoints = new List<Transform>();
    private List<BuildSnapPoint> childSnapScripts = new List<BuildSnapPoint>();
    bool isSnapped = false;


    [Header("Настройки Поворота")]
    public float RotationSnap = 5f;
    public bool AllowModeSwitch = true;
    public bool ManualRotation = false; // Выключает поворот постройки в сторону игрока, и дает ему возможность поварачивать на колесико мыши
    float cumulativeRotation = 0f;

    void Start()
    {
        if (SnapPointsParent != null)
        {
            foreach (Transform child in SnapPointsParent)
            {
                mySnapPoints.Add(child);
                childSnapScripts.Add(child.GetComponent<BuildSnapPoint>());
            }
        }

        TryGetComponent<MeshRenderer>(out MeshRenderer mesh);
        mesh.material = Available;
        modelBounds = mesh.bounds;
    }

    public void Place()
    {
        // TryGetComponent<MeshRenderer>(out MeshRenderer mesh);
        // mesh.enabled = false;
        // LockSchematic = true;

        Builder.instance.Placed();
        if (Build)
            Build.SetActive(true);

        Destroy(gameObject);
    }

    void Update()
    {
        if (!LockSchematic)
        {
            InputHandle();
            if (!isSnapped)
                RotationBehaviors();
            PositionBehaviors();
        }
    }

    void InputHandle()
    {
        GridMove = InputSystem.actions.FindAction("SnapToGrid").IsPressed(); // Включает режим привязки по сетке
        ToGround = InputSystem.actions.FindAction("SnapToGround").IsPressed(); // Опускает модель к земле
        
        cumulativeRotation += InputSystem.actions.FindAction("RotateBuild").ReadValue<float>();
        
        if (InputSystem.actions.FindAction("RotateBuild").inProgress && AllowModeSwitch && !ManualRotation)
        {
            // cumulativeRotation = Mathf.Round(transform.parent.eulerAngles.y / RotationSnap) * RotationSnap;
            ManualRotation = true; // Отключаю если пользователь хочет вращать колесиком мыши
        } 
        if (InputSystem.actions.FindAction("RotateToPlayer").inProgress && AllowModeSwitch && ManualRotation) 
            ManualRotation = false;
    }

    void RotationBehaviors()
    {
        if (ManualRotation)
        {
            transform.parent.rotation = Quaternion.AngleAxis(cumulativeRotation*RotationSnap, transform.parent.up);
            return;
        }
        
        transform.parent.rotation = Quaternion.LookRotation(PlayerCursor.instance.Anchor.forward, transform.parent.up);

        if(GridMove)
        {
            Vector3 fwd = PlayerCursor.instance.Anchor.forward;
            float yaw = Mathf.Atan2(fwd.x, fwd.z) * Mathf.Rad2Deg;
            
            float snappedYaw = Mathf.Round(yaw / 90f) * 90f;
            transform.parent.rotation = Quaternion.Euler(0f, snappedYaw, 0f);
        }
    }

    void PositionBehaviors()
    {
        // Свободное перемещение
        transform.parent.position = PlayerCursor.instance.Position;

        // Привязка по сетке
        if (GridMove)
        {
            Vector3 CursorPos = PlayerCursor.instance.Position;
            float newX = Mathf.Round(CursorPos.x / Grid.x) * Grid.x;
            float newY = Mathf.Round(CursorPos.y / Grid.y) * Grid.y;
            float newZ = Mathf.Round(CursorPos.z / Grid.z) * Grid.z;

            transform.parent.position = new Vector3(newX, newY, newZ);  
        }

        // Привязка к земле
        if (ToGround)
        {
            RaycastHit hit;
            if (Physics.Raycast(PlayerCursor.instance.Position, new Vector3(0, -1, 0), out hit, GroundDistance))
            {
                Vector3 Parent = transform.parent.position;
                transform.parent.position = new Vector3(Parent.x, hit.point.y, Parent.z);
            }
        }

        // Не дает модели утонуть в дерьме (земле, других постройках)
        if (DisableGroundClipping)
            KeepAboveGround();

        // Привязка по точкам (Если те есть) [ЭКСПЕРЕМЕНТАЛЬНО]
        if (SnapPointsParent != null && mySnapPoints.Count > 0)
        {
            SnapToStructure(); 
        }

    }


    public void KeepAboveGround()
    {
        Vector3 RayPos = PlayerCursor.instance.Position + new Vector3(0, 0.5f, 0);
        RaycastHit hit;
        if (Physics.SphereCast(RayPos, 0.1f, Vector3.down, out hit, 2*modelBounds.extents.y + 0.1f))
        {
            Vector3 clampedPos  = new Vector3(transform.parent.position.x, hit.point.y + modelBounds.extents.y, transform.parent.position.z);
            transform.parent.position = Vector3.Max(transform.parent.position, clampedPos);
        }
    }


    void SnapToStructure()
    {
        Collider[] colliders = Physics.OverlapSphere(PlayerCursor.instance.Position, .5f);
        
        Transform targetPoint = null; // Точка привязки
        Vector3 snapPoint = Vector3.zero; // Наша точка привязки
        float maxdifference = Mathf.Infinity; // Ищем SnapPoint с наиболее противонаправленным вектором
        BuildSnapPoint targetSnapScript = null;
        foreach (Collider col in colliders)
        {
            bool GETOUT = false;
            foreach (Transform myPoint in mySnapPoints)
            {
                if (myPoint == col.transform)
                {
                    GETOUT = true; // Нам не нужны наши же точки
                    break; 
                }
            }

            if (!col.TryGetComponent<BuildSnapPoint>(out targetSnapScript))
                GETOUT = true;
            
            if (GETOUT) 
                continue;

            targetPoint = targetSnapScript.transform;
        }

        if (!targetPoint)
        {
            isSnapped = false;
            return;
        }
    
        foreach (Transform myPoint in mySnapPoints)
        {
            float dotProd = Vector3.Dot((targetPoint.position - targetPoint.parent.position).normalized, (myPoint.position - SnapPointsParent.position).normalized);

            if (dotProd < maxdifference)
            {
                maxdifference = dotProd;
                snapPoint = myPoint.position;
            }
        }

        float rotationInDegrees = Vector3.SignedAngle(targetPoint.position - targetPoint.parent.position, snapPoint - SnapPointsParent.position, Vector3.down) - 180; // Доворачиваем объект
        Quaternion deltaRotation = Quaternion.AngleAxis(rotationInDegrees, Vector3.up);
        
        isSnapped = true;
        transform.parent.rotation = targetPoint.parent.rotation;
        transform.parent.position += targetPoint.position - snapPoint; // Фиксируем прибыль
        
        // else if (targetSnapScript.type == BuildSnapPoint.SnapType.Wall)
        // {
        //     float rotationInDegrees = Vector3.SignedAngle(target.position - target.parent.position, snapPoint.position - SnapPointsParent.position, Vector3.down) - 90; // Доворачиваем объект
        //     Quaternion deltaRotation = Quaternion.AngleAxis(rotationInDegrees, Vector3.up);
            
        //     transform.parent.rotation = deltaRotation * transform.parent.rotation;
        //     transform.parent.position += target.position - snapPoint.position; // Фиксируем прибыль
        // }

    }
}
