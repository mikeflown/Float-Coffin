using UnityEngine;

public class MonsterType1 : MonoBehaviour
{
    public bool isGoingLeft = true;
    public int currentFrame = 0;
    private float frameTimer = 0f;
    private float currentFrameDuration = 0f;
    private bool hasAttacked = false;
    [Header("Рандомное время для каждого кадра")]
    public float[] minFrameTimes = new float[4] { 2.5f, 1.8f, 1.2f, 0.8f };
    public float[] maxFrameTimes = new float[4] { 4.0f, 3.2f, 2.5f, 1.5f };
    public void Initialize(bool goingLeft, float[] minTimes, float[] maxTimes)
    {
        isGoingLeft = goingLeft;
        minFrameTimes = minTimes;
        maxFrameTimes = maxTimes;
        currentFrame = 0;
        frameTimer = 0f;
        hasAttacked = false;
        SetNewFrameDuration();
    }
    private void SetNewFrameDuration()
    {
        currentFrameDuration = Random.Range(minFrameTimes[currentFrame], maxFrameTimes[currentFrame]);
    }
    private void Update()
    {
        if (hasAttacked) return;
        frameTimer += Time.deltaTime;
        if (frameTimer >= currentFrameDuration)
        {
            frameTimer = 0f;
            currentFrame++;
            if (currentFrame >= 4)
            {
                Attack();
            }
            else
            {
                SetNewFrameDuration();
            }
        }
    }
    private void Attack()
    {
        hasAttacked = true;
        if (SubmarineHealth.Instance != null)
            SubmarineHealth.Instance.TakeDamage(1);
    }
}