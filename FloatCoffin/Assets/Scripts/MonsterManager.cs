using UnityEngine;
using UnityEngine.Events;

public class MonsterManager : MonoBehaviour
{
    public static MonsterManager Instance { get; private set; }
    [Header("UI Панели")]
    public GameObject underWaterLeft;
    public GameObject underWaterRight;
    public GameObject centerMonsterVisual;
    [Header("Радар")]
    public RectTransform radarPanel;
    public GameObject radarBlipPrefab;      // маленький блик для Type 2
    public GameObject bigRedBlipPrefab;     // большой красный блик для Type 3
    [Header("Настройки спавна")]
    public float minSpawnInterval = 10f;
    public float maxSpawnInterval = 25f;
    private float nextSpawnTime;
    private Monster leftMonster;
    private Monster rightMonster;
    private Monster centerMonster;
    public UnityEvent onCloseLeft;
    public UnityEvent onCloseRight;
    public UnityEvent onCloseCenter;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    private void Start()
    {
        leftMonster = CreateMonster("LeftMonster");
        rightMonster = CreateMonster("RightMonster");
        nextSpawnTime = Time.time + 8f;
    }
    private Monster CreateMonster(string name)
    {
        GameObject go = new GameObject(name);
        go.transform.SetParent(transform);
        Monster monster = go.AddComponent<Monster>();
        return monster;
    }
    private void Update()
    {
        if (leftMonster != null)   leftMonster.UpdateDistance(Time.deltaTime);
        if (rightMonster != null)  rightMonster.UpdateDistance(Time.deltaTime);
        if (centerMonster != null) centerMonster.UpdateDistance(Time.deltaTime);
        UpdateAllVisuals();
        if (Time.time >= nextSpawnTime)
        {
            SpawnRandomMonster();
            nextSpawnTime = Time.time + Random.Range(minSpawnInterval, maxSpawnInterval);
        }
    }
    private void UpdateAllVisuals()
    {
        UpdateSidePanel(leftMonster, underWaterLeft);
        UpdateSidePanel(rightMonster, underWaterRight);
        if (centerMonsterVisual != null)
        {
            bool show = centerMonster != null && 
                        centerMonster.currentDistance != DistanceState.Close &&
                        centerMonster.type == MonsterType.Type3_Central;

            centerMonsterVisual.SetActive(show && AreHeadlightsOn());
        }
        UpdateRadar();
    }
    private void UpdateSidePanel(Monster monster, GameObject panel)
    {
        if (panel == null || monster == null) return;

        bool visible = (monster.currentDistance == DistanceState.Medium) ||
                       (monster.currentDistance == DistanceState.Close && monster.type != MonsterType.Type2_RadarOnly);

        panel.SetActive(visible);
    }
    private bool AreHeadlightsOn() => true; // ← потом заменишь
    private void UpdateRadar()
    {
        if (radarPanel == null) return;
        // Очищаем старые блики
        foreach (Transform child in radarPanel.transform)
            Destroy(child.gameObject);
        // Type 2 — всегда маленький блик
        if (leftMonster != null && leftMonster.IsVisibleOnRadar())
            CreateBlip(leftMonster, false);
        if (rightMonster != null && rightMonster.IsVisibleOnRadar())
            CreateBlip(rightMonster, false);
        // Type 3 Close — большой красный блик
        if (centerMonster != null && centerMonster.IsVisibleOnRadar())
            CreateBlip(centerMonster, true);
    }
    private void CreateBlip(Monster monster, bool isBigRed)
    {
        GameObject prefab = isBigRed ? bigRedBlipPrefab : radarBlipPrefab;
        if (prefab == null) return;

        GameObject blip = Instantiate(prefab, radarPanel);

        RectTransform rt = blip.GetComponent<RectTransform>();
        if (rt == null) return;

        Vector2 pos = monster.side == Monster.Side.Left ? new Vector2(-50, 0) : new Vector2(50, 0);
        float closeness = monster.currentDistance == DistanceState.Close ? 0.2f : 
                          monster.currentDistance == DistanceState.Medium ? 0.6f : 0.9f;

        rt.anchoredPosition = pos * closeness;
    }
    public void RepelLeft()  { if (leftMonster  != null) leftMonster.ResetMonster(MonsterType.Type1_Visual, Monster.Side.Left); }
    public void RepelRight() { if (rightMonster != null) rightMonster.ResetMonster(MonsterType.Type1_Visual, Monster.Side.Right); }
    public void RepelCenter()
    {
        if (centerMonster != null)
        {
            LevelProgressManager.Instance?.RollbackProgress(15f);
            centerMonster = null;
        }
    }
    private void SpawnRandomMonster()
    {
        int r = Random.Range(0, 100);
        if (r < 40)
            leftMonster.ResetMonster(Random.value > 0.5f ? MonsterType.Type1_Visual : MonsterType.Type2_RadarOnly, Monster.Side.Left);
        else if (r < 80)
            rightMonster.ResetMonster(Random.value > 0.5f ? MonsterType.Type1_Visual : MonsterType.Type2_RadarOnly, Monster.Side.Right);
        else
        {
            if (centerMonster == null)
                centerMonster = CreateMonster("CenterMonster");
            centerMonster.ResetMonster(MonsterType.Type3_Central, Monster.Side.Center, 0.5f);
        }
    }
}