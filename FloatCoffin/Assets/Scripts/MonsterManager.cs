using UnityEngine;
using UnityEngine.Events;

public class MonsterManager : MonoBehaviour
{
    public static MonsterManager Instance { get; private set; }
    [Header("Контейнеры и визуалы")]
    public GameObject monsterVisualLeft;
    public GameObject monsterVisualRight;
    public GameObject monsterVisualCenter;
    [Header("Радар")]
    public RectTransform radarPanel;
    public GameObject radarBlipPrefab;
    public GameObject bigRedBlipPrefab;
    [Header("Настройки спавна")]
    public float minSpawnInterval = 10f;
    public float maxSpawnInterval = 25f;
    private float nextSpawnTime;
    private Monster activeLeftMonster;
    private Monster activeRightMonster;
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
        nextSpawnTime = Time.time + 5f;
    }
    private Monster CreateNewMonster()
    {
        GameObject go = new GameObject("Monster");
        go.transform.SetParent(transform);
        return go.AddComponent<Monster>();
    }
    private void Update()
    {
        if (activeLeftMonster != null)   activeLeftMonster.UpdateDistance(Time.deltaTime);
        if (activeRightMonster != null)  activeRightMonster.UpdateDistance(Time.deltaTime);
        if (centerMonster != null)       centerMonster.UpdateDistance(Time.deltaTime);
        UpdateAllVisuals();
        if (Time.time >= nextSpawnTime)
        {
            SpawnRandomMonster();
            nextSpawnTime = Time.time + Random.Range(minSpawnInterval, maxSpawnInterval);
        }
    }
    private void UpdateAllVisuals()
    {
        UpdateSideVisual(activeLeftMonster, monsterVisualLeft);
        UpdateSideVisual(activeRightMonster, monsterVisualRight);
        UpdateCenterVisual();
        UpdateRadar();
    }
    private void UpdateSideVisual(Monster monster, GameObject visualObject)
    {
        if (visualObject == null || monster == null) 
        {
            if (visualObject != null) visualObject.SetActive(false);
            return;
        }
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
                          centerMonster.currentDistance != DistanceState.Close;
        monsterVisualCenter.SetActive(shouldShow && AreHeadlightsOn());
    }
    private bool AreHeadlightsOn() => true;
    private void UpdateRadar()
    {
        if (radarPanel == null) return;
        foreach (Transform child in radarPanel) Destroy(child.gameObject);
        if (activeLeftMonster != null && activeLeftMonster.IsVisibleOnRadar())
            CreateRadarBlip(activeLeftMonster);
        if (activeRightMonster != null && activeRightMonster.IsVisibleOnRadar())
            CreateRadarBlip(activeRightMonster);
        if (centerMonster != null && centerMonster.IsVisibleOnRadar())
            CreateRadarBlip(centerMonster, true);
    }
    private void CreateRadarBlip(Monster monster, bool isBigRed = false)
    {
        GameObject prefab = isBigRed ? bigRedBlipPrefab : radarBlipPrefab;
        if (prefab == null) return;
        GameObject blip = Instantiate(prefab, radarPanel);
        RectTransform rt = blip.GetComponent<RectTransform>();
        // Позиция зависит от стороны
        Vector2 basePos = (monster.side == Monster.Side.Left) ? new Vector2(-55, 0) : new Vector2(55, 0);
        float closeness = monster.currentDistance == DistanceState.Close ? 0.15f :
                          monster.currentDistance == DistanceState.Medium ? 0.55f : 0.9f;
        rt.anchoredPosition = basePos * closeness;
    }
    // ====================== СПАВН МОНСТРОВ ======================
    private void SpawnRandomMonster()
    {
        int roll = Random.Range(0, 100);
        if (roll < 45) // Левый или Правый монстр (Type 1 или 2)
        {
            bool isLeft = Random.value > 0.5f;
            MonsterType type = Random.value > 0.5f ? MonsterType.Type1_Visual : MonsterType.Type2_RadarOnly;
            Monster newMonster = CreateNewMonster();
            newMonster.ResetMonster(type, isLeft ? Monster.Side.Left : Monster.Side.Right);
            if (isLeft)
            {
                if (activeLeftMonster != null) Destroy(activeLeftMonster.gameObject);
                activeLeftMonster = newMonster;
            }
            else
            {
                if (activeRightMonster != null) Destroy(activeRightMonster.gameObject);
                activeRightMonster = newMonster;
            }
        }
        else // Центр — только Type 3
        {
            if (centerMonster == null)
                centerMonster = CreateNewMonster();
            centerMonster.ResetMonster(MonsterType.Type3_Central, Monster.Side.Center, 0.45f);
        }
    }
    // ====================== ОТПУГИВАНИЕ ======================
    public void RepelLeft()
    {
        if (activeLeftMonster != null)
        {
            activeLeftMonster.ResetMonster(MonsterType.Type1_Visual, Monster.Side.Left);
            onCloseLeft?.Invoke();
        }
    }
    public void RepelRight()
    {
        if (activeRightMonster != null)
        {
            activeRightMonster.ResetMonster(MonsterType.Type1_Visual, Monster.Side.Right);
            onCloseRight?.Invoke();
        }
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
}