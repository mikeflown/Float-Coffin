using UnityEngine;

public class MonsterType3 : MonoBehaviour
{
    public int currentPhase = 0;
    private float phaseTimer = 0f;
    private float currentPhaseDuration = 0f;
    private bool hasAttacked = false;
    [Header("Настройки отката")]
    public float rollbackAmount = 30f;
    [Header("Рандомное время фаз (3 фазы)")]
    public float[] minPhaseTimes = new float[3] { 5.0f, 3.5f, 2.0f };
    public float[] maxPhaseTimes = new float[3] { 8.0f, 5.5f, 3.5f };
    public void Initialize(float[] minTimes, float[] maxTimes)
    {
        minPhaseTimes = minTimes;
        maxPhaseTimes = maxTimes;
        currentPhase = 0;
        phaseTimer = 0f;
        hasAttacked = false;
        SetNewPhaseDuration();
    }
    private void SetNewPhaseDuration()
    {
        currentPhaseDuration = Random.Range(minPhaseTimes[currentPhase], maxPhaseTimes[currentPhase]);
    }
    private void Update()
    {
        if (hasAttacked) return;
        phaseTimer += Time.deltaTime;
        if (phaseTimer >= currentPhaseDuration)
        {
            phaseTimer = 0f;
            currentPhase++;
            if (currentPhase >= 3)
            {
                Attack();
            }
            else
            {
                SetNewPhaseDuration();
            }
        }
    }
    private void Attack()
    {
        hasAttacked = true;
        if (LevelProgressManager.Instance != null)
            LevelProgressManager.Instance.RollbackProgress(rollbackAmount);
        Debug.Log($"Type 3 атаковал! Откат на {rollbackAmount} единиц.");
    }
}