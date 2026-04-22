using UnityEngine;

public class MonsterManager : MonoBehaviour
{
    public static MonsterManager Instance { get; private set; }

    // ==================== TYPE 1 ====================
    [Header("Type 1 Настройки")]
    public float type1MinSpawnTime = 12f;
    public float type1MaxSpawnTime = 25f;
    public float[] type1MinFrameTimes = new float[4] { 2.5f, 1.8f, 1.2f, 0.8f };
    public float[] type1MaxFrameTimes = new float[4] { 4.0f, 3.2f, 2.5f, 1.5f };
    [Header("Type 1 Визуалы")]
    public GameObject silhouetteLeft;
    public GameObject silhouetteRight;
    public GameObject[] leftFramesType1 = new GameObject[3];
    public GameObject[] rightFramesType1 = new GameObject[3];

    // ==================== TYPE 2 ====================
    [Header("Type 2 Настройки")]
    public float type2MinSpawnTime = 15f;
    public float type2MaxSpawnTime = 30f;
    public float[] type2MinPhaseTimes = new float[4] { 3.0f, 2.5f, 2.0f, 1.2f };
    public float[] type2MaxPhaseTimes = new float[4] { 5.5f, 4.5f, 3.5f, 2.0f };
    [Header("Type 2 Стробоскоп")]
    public GameObject leftFrameType2_Phase2;
    public GameObject leftFrameType2_Phase3;
    public GameObject rightFrameType2_Phase2;
    public GameObject rightFrameType2_Phase3;
    [Header("Type 2 Радар")]
    public GameObject[] leftRadarImages = new GameObject[4];
    public GameObject[] rightRadarImages = new GameObject[4];
    private MonsterType1 currentType1;
    private MonsterType2 currentType2;
    private float nextType1Spawn;
    private float nextType2Spawn;
    private void Awake() => Instance = this;
    private void Start()
    {
        nextType1Spawn = Time.time + 8f;
        nextType2Spawn = Time.time + 12f;
    }
    private void Update()
    {
        UpdateVisuals();
        if (Time.time >= nextType1Spawn)
        {
            SpawnType1();
            nextType1Spawn = Time.time + Random.Range(type1MinSpawnTime, type1MaxSpawnTime);
        }
        if (Time.time >= nextType2Spawn)
        {
            SpawnType2();
            nextType2Spawn = Time.time + Random.Range(type2MinSpawnTime, type2MaxSpawnTime);
        }
    }
    private void SpawnType1()
    {
        if (currentType1 != null) Destroy(currentType1.gameObject);
        GameObject go = new GameObject("Type1");
        currentType1 = go.AddComponent<MonsterType1>();
        bool goingLeft = Random.Range(0, 2) == 0;
        currentType1.Initialize(goingLeft, type1MinFrameTimes, type1MaxFrameTimes);
    }
    private void SpawnType2()
    {
        if (currentType2 != null) Destroy(currentType2.gameObject);
        GameObject go = new GameObject("Type2");
        currentType2 = go.AddComponent<MonsterType2>();
        bool goingLeft = Random.Range(0, 2) == 0;
        currentType2.Initialize(goingLeft, type2MinPhaseTimes, type2MaxPhaseTimes);
    }
    private void UpdateVisuals()
    {
        if (silhouetteLeft) silhouetteLeft.SetActive(false);
        if (silhouetteRight) silhouetteRight.SetActive(false);
        foreach (var f in leftFramesType1) if (f) f.SetActive(false);
        foreach (var f in rightFramesType1) if (f) f.SetActive(false);
        if (leftFrameType2_Phase2) leftFrameType2_Phase2.SetActive(false);
        if (leftFrameType2_Phase3) leftFrameType2_Phase3.SetActive(false);
        if (rightFrameType2_Phase2) rightFrameType2_Phase2.SetActive(false);
        if (rightFrameType2_Phase3) rightFrameType2_Phase3.SetActive(false);
        foreach (var img in leftRadarImages) if (img) img.SetActive(false);
        foreach (var img in rightRadarImages) if (img) img.SetActive(false);
        
        // === TYPE 1 ===
        if (currentType1 != null)
        {
            int frame = currentType1.currentFrame;
            bool left = currentType1.isGoingLeft;

            if (frame == 0)
            {
                if (silhouetteLeft) silhouetteLeft.SetActive(left);
                if (silhouetteRight) silhouetteRight.SetActive(!left);
            }
            else if (frame >= 1 && frame <= 3 && frame - 1 < leftFramesType1.Length)
            {
                int idx = frame - 1;
                if (left && leftFramesType1[idx]) leftFramesType1[idx].SetActive(true);
                if (!left && rightFramesType1[idx]) rightFramesType1[idx].SetActive(true);
            }
        }
        // === TYPE 2 ===
        if (currentType2 != null)
        {
            int phase = currentType2.currentPhase;
            bool left = currentType2.isGoingLeft;
            if (phase == 2)
            {
                if (left && leftFrameType2_Phase2) leftFrameType2_Phase2.SetActive(true);
                if (!left && rightFrameType2_Phase2) rightFrameType2_Phase2.SetActive(true);
            }
            else if (phase == 3)
            {
                if (left && leftFrameType2_Phase3) leftFrameType2_Phase3.SetActive(true);
                if (!left && rightFrameType2_Phase3) rightFrameType2_Phase3.SetActive(true);
            }
            // === РАДАР ===
            if (phase >= 0 && phase < 4)
            {
                if (left && phase < leftRadarImages.Length && leftRadarImages[phase] != null)
                    leftRadarImages[phase].SetActive(true);

                if (!left && phase < rightRadarImages.Length && rightRadarImages[phase] != null)
                    rightRadarImages[phase].SetActive(true);
            }
        }
    }
}