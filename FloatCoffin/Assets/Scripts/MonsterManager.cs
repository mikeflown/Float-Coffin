using UnityEngine;

public class MonsterManager : MonoBehaviour
{
    public static MonsterManager Instance { get; private set; }
    [Header("Главный экран (только кадр 0)")]
    public GameObject silhouetteLeft;
    public GameObject silhouetteRight;
    [Header("Левый стробоскоп (кадры 1, 2, 3)")]
    public GameObject[] leftFrames = new GameObject[3];
    [Header("Правый стробоскоп (кадры 1, 2, 3)")]
    public GameObject[] rightFrames = new GameObject[3];
    [Header("Настройки анимации")]
    [Header("Время кадра")]
    public float[] minFrameTimes = new float[4] { 2.5f, 1.8f, 1.2f, 0.8f };
    public float[] maxFrameTimes = new float[4] { 4.0f, 3.2f, 2.5f, 1.5f };
    public float[] frameDurations = new float[4] { 1.5f, 1.2f, 1.0f, 0.8f };
    [Header("Спавн")]
    public float minSpawnTime = 12f;
    public float maxSpawnTime = 25f;
    private MonsterType1 currentMonster;
    private float nextSpawnTime;
    private void Awake() => Instance = this;
    private void Start()
    {
        nextSpawnTime = Time.time + 5f;
    }
    private void Update()
    {
        if (currentMonster != null)
            UpdateVisuals();
        if (Time.time >= nextSpawnTime)
        {
            SpawnType1();
            nextSpawnTime = Time.time + Random.Range(minSpawnTime, maxSpawnTime);
        }
    }
    private void SpawnType1()
    {
        if (currentMonster != null) Destroy(currentMonster.gameObject);
        GameObject go = new GameObject("Type1_Monster");
        currentMonster = go.AddComponent<MonsterType1>();
        bool goingLeft = Random.Range(0, 2) == 0;
        currentMonster.Initialize(goingLeft, minFrameTimes, maxFrameTimes);
    }
    private void UpdateVisuals()
    {
        if (currentMonster == null) return;
        int frame = currentMonster.currentFrame;
        bool left = currentMonster.isGoingLeft;
        bool showSilhouette = (frame == 0);
        if (silhouetteLeft)  silhouetteLeft.SetActive(showSilhouette && left);
        if (silhouetteRight) silhouetteRight.SetActive(showSilhouette && !left);
        foreach (var obj in leftFrames)  if (obj) obj.SetActive(false);
        foreach (var obj in rightFrames) if (obj) obj.SetActive(false);
        if (frame >= 1 && frame <= 3)
        {
            int stroboscopeIndex = frame - 1;
            if (left && leftFrames[stroboscopeIndex])
                leftFrames[stroboscopeIndex].SetActive(true);
            if (!left && rightFrames[stroboscopeIndex])
                rightFrames[stroboscopeIndex].SetActive(true);
        }
    }
}