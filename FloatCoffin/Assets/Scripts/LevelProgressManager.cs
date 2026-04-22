using UnityEngine;
using UnityEngine.Events;

public class LevelProgressManager : MonoBehaviour
{
    public static LevelProgressManager Instance { get; private set; }
    [Header("Настройки уровня")]
    [Tooltip("Общая длина уровня")]
    public float levelLength = 100f;
    [Tooltip("Скорость продвижения при работающем двигателе")]
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
            WinLevel();
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
    private void WinLevel()
    {
        isEngineWorking = false;
        enabled = false;
        Debug.Log("ПОБЕДА!");
        // Здесь позже добавим красивый экран победы
        Time.timeScale = 0f;
    }
    public float GetNormalizedProgress() => currentProgress / levelLength;
}