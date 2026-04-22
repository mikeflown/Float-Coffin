using UnityEngine;

public class MonsterType2 : MonoBehaviour
{
    public bool isGoingLeft = true;
    public int currentPhase = 0;
    private float phaseTimer = 0f;
    private float currentPhaseDuration = 0f;
    private bool hasAttacked = false;
    [Header("Рандомное время для КАЖДОЙ фазы")]
    public float[] minPhaseTimes = new float[4] { 3.0f, 2.5f, 2.0f, 1.2f };
    public float[] maxPhaseTimes = new float[4] { 5.5f, 4.5f, 3.5f, 2.0f };
    public void Initialize(bool goingLeft, float[] minTimes, float[] maxTimes)
    {
        isGoingLeft = goingLeft;
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
            if (currentPhase >= 4)
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
        if (SubmarineHealth.Instance != null)
            SubmarineHealth.Instance.TakeDamage(1);
    }
    public bool ShouldShowOnStroboscope() => currentPhase == 3;
}