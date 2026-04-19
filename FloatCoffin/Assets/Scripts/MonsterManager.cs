using UnityEngine;
using UnityEngine.Events;

public class MonsterManager : MonoBehaviour
{
    public static MonsterManager Instance { get; private set; }
    [Header("Контейнеры экранов (всегда активны)")]
    public GameObject underWaterLeftContainer;
    public GameObject underWaterRightContainer;
    [Header("Объекты с картинкой монстра")]
    public GameObject monsterVisualLeft;
    public GameObject monsterVisualRight;
    public GameObject monsterVisualCenter;
    [Header("Радар")]
    public RectTransform radarPanel;
    public GameObject radarBlipPrefab;
    public GameObject bigRedBlipPrefab;
    [Header("Настройки")]
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
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
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
        return go.AddComponent<Monster>();
    }
    private void Update()
    {
        if (leftMonster != null)   leftMonster.UpdateDistance(Time.deltaTime);
        if (rightMonster != null)  rightMonster.UpdateDistance(Time.deltaTime);
        if (centerMonster != null) centerMonster.UpdateDistance(Time.deltaTime);
        UpdateVisuals();
        if (Time.time >= nextSpawnTime)
        {
            SpawnRandomMonster();
            nextSpawnTime = Time.time + Random.Range(minSpawnInterval, maxSpawnInterval);
        }
    }
    private void UpdateVisuals()
    {
        UpdateMonsterVisual(leftMonster, monsterVisualLeft);
        UpdateMonsterVisual(rightMonster, monsterVisualRight);
        UpdateCenterVisual();
        UpdateRadar();
    }
    private void UpdateMonsterVisual(Monster monster, GameObject visualObject)
    {
        if (visualObject == null || monster == null) return;
        bool shouldShow = false;
        if (monster.type == MonsterType.Type2_RadarOnly)
        {
            shouldShow = monster.currentDistance == DistanceState.Close;
        }
        else
        {
            shouldShow = monster.currentDistance == DistanceState.Medium ||
                         monster.currentDistance == DistanceState.Close;
        }
        visualObject.SetActive(shouldShow);
    }
    private void UpdateCenterVisual()
    {
        if (monsterVisualCenter == null) return;
        bool shouldShow = centerMonster != null &&
                          centerMonster.type == MonsterType.Type3_Central &&
                          centerMonster.currentDistance != DistanceState.Close; // далеко или средне
        monsterVisualCenter.SetActive(shouldShow && AreHeadlightsOn());
    }
    private bool AreHeadlightsOn() => true; // ← позже подключишь реальную систему фар
    private void UpdateRadar()
    {
        if (radarPanel == null) return;
        // Очищаем предыдущие блики
        foreach (Transform child in radarPanel)
            Destroy(child.gameObject);
        // Type 2 — всегда виден на радаре
        if (leftMonster != null && leftMonster.IsVisibleOnRadar())
            CreateRadarBlip(leftMonster, false);
        if (rightMonster != null && rightMonster.IsVisibleOnRadar())
            CreateRadarBlip(rightMonster, false);
        // Type 3 — только когда Close, большой красный
        if (centerMonster != null && centerMonster.IsVisibleOnRadar())
            CreateRadarBlip(centerMonster, true);
    }
    private void CreateRadarBlip(Monster monster, bool isBigRed)
    {
        GameObject prefab = isBigRed ? bigRedBlipPrefab : radarBlipPrefab;
        if (prefab == null) return;

        GameObject blip = Instantiate(prefab, radarPanel);
        RectTransform rt = blip.GetComponent<RectTransform>();

        Vector2 basePos = monster.side == Monster.Side.Left ? new Vector2(-55, 0) : new Vector2(55, 0);

        float closeness = monster.currentDistance == DistanceState.Close ? 0.15f :
                          monster.currentDistance == DistanceState.Medium ? 0.55f : 0.9f;

        rt.anchoredPosition = basePos * closeness;
    }
    // ====================== Публичные методы для кнопок ======================
    public void RepelLeft()
    {
        if (leftMonster != null)
            leftMonster.ResetMonster(MonsterType.Type1_Visual, Monster.Side.Left);
        onCloseLeft?.Invoke();
    }
    public void RepelRight()
    {
        if (rightMonster != null)
            rightMonster.ResetMonster(MonsterType.Type1_Visual, Monster.Side.Right);
        onCloseRight?.Invoke();
    }
    public void RepelCenter()
    {
        if (centerMonster != null)
        {
            LevelProgressManager.Instance?.RollbackProgress(15f);
            centerMonster = null;
            onCloseCenter?.Invoke();
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
            centerMonster.ResetMonster(MonsterType.Type3_Central, Monster.Side.Center, 0.45f);
        }
    }
}