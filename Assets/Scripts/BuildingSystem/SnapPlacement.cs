using UnityEngine;
using System.Collections.Generic;
using System.Linq;


public class SnapPlacement : MonoBehaviour
{
    [Header("Настройки Привязки")]
    [SerializeField] Transform SnapPointsParent; 
    public bool isSnapped = false;
    
    private List<Transform> SnapPointsTransform = new List<Transform>();
    private List<BuildSnapPoint> SnapPointsScripts = new List<BuildSnapPoint>();

    void Awake()
    {
        if (SnapPointsParent == null) {
            enabled = false;
            return;
        }
        
        foreach (Transform child in SnapPointsParent) { 
            SnapPointsTransform.Add(child);
            SnapPointsScripts.Add(child.GetComponent<BuildSnapPoint>());
        }
    }

    // дальше бога нет.
    void Update()
    {
        BuildSnapPoint targetPoint = null; // Найденная точка привязки (Другой постройки)
        
        BuildSnapPoint bestPoint = null; // Подходящая точка привязки (Принадлежащая нашей постройке)

        targetPoint = PlayerCursor.instance.CollidedTrigger.Select(x => x.collider.GetComponent<BuildSnapPoint>())
        .Where(x => x != null)
        .FirstOrDefault(x => !SnapPointsScripts.Contains(x));

        if (!targetPoint)
        {
            isSnapped = false;
            return;
        }

        #region Находим такую точку, которая лежала бы противоположно targetPoint.

        if (SnapPointsScripts.Any(p => p.Orientation == targetPoint.Orientation)) {
            bestPoint = FindBestSnapPoint(targetPoint.SnapType, targetPoint.PointDirection);
        }
        else {
            Vector3 targetParent = targetPoint.transform.parent.position;

            // 1 Случай. Присоединяем вертикальную постройку к горизонтальной 
            // В данном случае смотрим на положение мыши, если она ниже центра target постройки, присоединяем снизу
            // Если она выше, то присоединяем сверху 
            if (targetPoint.Orientation == BuildSnapPoint.Orientations.Horizontal)
            {
                Vector3 cursorPoint = PlayerCursor.instance.CollidedTrigger.Where(x => targetPoint == x.collider.GetComponent<BuildSnapPoint>()).FirstOrDefault().point;

                if (cursorPoint.y > targetParent.y)
                    bestPoint = SnapPointsScripts.OrderBy(p => p.transform.position.y).FirstOrDefault(); // Пока поломано оставлю та
                else
                    bestPoint = SnapPointsScripts.OrderByDescending(p => p.transform.position.y).FirstOrDefault();
            }

            // 2 Случай. Присоединяем горизонтальную постройку к вертикальной
            // Я не хочу объяснять, если надо спросите меня напрямую.
            if (targetPoint.Orientation == BuildSnapPoint.Orientations.Vertical)
            {
                Vector3 cursorDir = (PlayerCursor.instance.Anchor.position - targetParent).normalized;
                float projection = Vector3.Dot(targetPoint.transform.root.forward, cursorDir);
                int flip = projection > 0 ? 1 : -1; // Когда заходим за другую сторону стенки, отражаем вектор.

                bestPoint = FindBestSnapPoint(targetPoint.SnapType, targetPoint.transform.root.forward * flip);
            }
        }

        #endregion

        if (!bestPoint) 
        {
            isSnapped = false;
            return;
        }

        float targetYaw = targetPoint.transform.root.eulerAngles.y;
        float currentYaw = transform.eulerAngles.y;
        float deltaYaw = Mathf.DeltaAngle(currentYaw, targetYaw);
        float snappedYaw = Mathf.RoundToInt(deltaYaw / bestPoint.RotationSnap) * bestPoint.RotationSnap;

        transform.rotation = Quaternion.Euler(0, targetYaw - snappedYaw, 0);
        
        transform.position += targetPoint.transform.position - bestPoint.transform.position;
        isSnapped = true;
    }

    // Находит подходящую точку для привязке, основываясь на скалярном произведении.
    // В случае если точки лежат в одном пространстве (т.е BuildType одинаковый), это дает ожидаемый и оптимальный результат.
    BuildSnapPoint FindBestSnapPoint(BuildSnapPoint.SnapTypes type, Vector3 vec)
    {
        return SnapPointsScripts.Where(p => p.SnapType == type)
                .OrderBy(p => Vector3.Dot(p.PointDirection, vec))
                .ThenBy(p => -p.transform.position.sqrMagnitude)
                .FirstOrDefault();
    }
}
