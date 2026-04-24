using UnityEngine;

public class DepthIndicator : MonoBehaviour
{
    [Header("Ссылка на менеджер")]
    public LevelProgressManager levelProgressManager;
    [Header("Настройки движения точки")]
    [Tooltip("Самая верхняя позиция точки (начало уровня)")]
    public RectTransform topPosition;
    [Tooltip("Самая нижняя позиция точки (конец уровня)")]
    public RectTransform bottomPosition;
    [Tooltip("Ссылка на сам объект Point")]
    public RectTransform pointTransform;
    private void OnEnable()
    {
        if (levelProgressManager != null)
            levelProgressManager.onProgressChanged.AddListener(UpdatePointPosition);
    }
    private void OnDisable()
    {
        if (levelProgressManager != null)
            levelProgressManager.onProgressChanged.RemoveListener(UpdatePointPosition);
    }
    private void UpdatePointPosition(float normalized)
    {
        if (pointTransform == null || topPosition == null || bottomPosition == null) 
            return;
        float newY = Mathf.Lerp(topPosition.anchoredPosition.y, bottomPosition.anchoredPosition.y, normalized);
        Vector2 newPos = pointTransform.anchoredPosition;
        newPos.y = newY;
        pointTransform.anchoredPosition = newPos;
    }
}