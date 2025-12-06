using UnityEngine;


// Попытка больше абстрагировать код
public interface IPlacementStrategy
{
    Vector3 CalculatePosition(PlayerCursor cursor);
    Quaternion CalculateRotation(PlayerCursor cursor);
}

public abstract class IPlacementModifier
{
    // enum ModifierType { Sum, Change };

    // ModifierType modifierType = ModifierType.Sum;

    public abstract Vector3 ModifyPosition(PlayerCursor cursor, Vector3 CurrentPos);
    public abstract Quaternion ModifyRotation(PlayerCursor cursor, Quaternion CurrentRot);
}

public class StandardPlacement : IPlacementStrategy
{
    Vector3 IPlacementStrategy.CalculatePosition(PlayerCursor cursor) => cursor.Position;

    Quaternion IPlacementStrategy.CalculateRotation(PlayerCursor cursor) => Quaternion.LookRotation(cursor.Anchor.forward, cursor.Anchor.transform.up);
}

public class GridPlacement : IPlacementStrategy
{
    public Vector3 Grid;
    public float RotationStep = 90f;
    Vector3 IPlacementStrategy.CalculatePosition(PlayerCursor cursor)
    {
        Vector3 CursorPos = cursor.Position;
        float newX = Mathf.Round(CursorPos.x / Grid.x) * Grid.x;
        float newY = Mathf.Round(CursorPos.y / Grid.y) * Grid.y;
        float newZ = Mathf.Round(CursorPos.z / Grid.z) * Grid.z;

        return new Vector3(newX, newY, newZ);  
    }

    Quaternion IPlacementStrategy.CalculateRotation(PlayerCursor cursor)
    {
        Vector3 fwd = cursor.Anchor.forward;
        float yaw = Mathf.Atan2(fwd.x, fwd.z) * Mathf.Rad2Deg;
        
        float snappedYaw = Mathf.Round(yaw / 90f) * 90f;
        return Quaternion.Euler(0f, snappedYaw, 0f);
    }
}

public class SnapToGround : IPlacementModifier
{
    float GroundDistance = 2f;
    public override Vector3 ModifyPosition(PlayerCursor cursor, Vector3 CurrentPos)
    {
        RaycastHit hit;
        if (Physics.Raycast(PlayerCursor.instance.Position, new Vector3(0, -1, 0), out hit, GroundDistance))
        {
            return new Vector3(CurrentPos.x, hit.point.y, CurrentPos.z);
        }
        return Vector3.zero;
    }

    public override Quaternion ModifyRotation(PlayerCursor cursor, Quaternion CurrentRot) => throw new System.NotImplementedException();
}