using UnityEngine;
using UnityEngine.Events;

public class LevelProgressManager : MonoBehaviour
{
    public static LevelProgressManager Instance { get; private set; }
    [Header("Настройки уровня")]
    [Tooltip("Общая длина уровня (чем больше — тем длиннее уровень)")]
    public float levelLength = 100f;
    [Tooltip("Скорость продвижения при работающем двигателе (единиц в секунду)")]
    public float baseProgressSpeed = 8f;
    [Header("Текущее состояние")]
    public float currentProgress = 0f;
    public bool isEngineWorking = true;
    public UnityEvent<float> onProgressChanged;
    public UnityEvent onLevelCompleted;
    public UnityEvent<float> onSubmarineBumped;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    private void Update()
    {
        if (!isEngineWorking) return;

        currentProgress += baseProgressSpeed * Time.deltaTime;

        if (currentProgress >= levelLength)
        {
            currentProgress = levelLength;
            onLevelCompleted?.Invoke();
            enabled = false;
            return;
        }

        float normalized = Mathf.Clamp01(currentProgress / levelLength);
        onProgressChanged?.Invoke(normalized);
    }
    public void RollbackProgress(float amount)
    {
        currentProgress = Mathf.Max(0f, currentProgress - amount);
        float normalized = currentProgress / levelLength;
        onProgressChanged?.Invoke(normalized);
        onSubmarineBumped?.Invoke(amount);
    }
    public void SetEngineState(bool working)
    {
        isEngineWorking = working;
    }
    public void ResetProgress()
    {
        currentProgress = 0f;
        isEngineWorking = true;
        enabled = true;
    }
    public float GetNormalizedProgress() => currentProgress / levelLength;
}