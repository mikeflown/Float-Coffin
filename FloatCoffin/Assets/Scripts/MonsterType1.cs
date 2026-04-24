using UnityEngine;

public class MonsterType1 : MonoBehaviour
{
    public bool isGoingLeft = true;
    public int currentFrame = 0;
    private float frameTimer = 0f;
    private float currentFrameDuration = 0f;
    private bool hasAttacked = false;
    [Header("Рандомный промежуток времени для каждой фазы")]
    public float[] minFrameTimes = new float[4] { 4f, 8f, 5f, 2f };
    public float[] maxFrameTimes = new float[4] { 8f, 10f, 7f, 4f };
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