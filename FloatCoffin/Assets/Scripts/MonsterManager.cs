using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class MonsterManager : MonoBehaviour
{
    public static MonsterManager Instance { get; private set; }
    [Header("Визуалы")]
    public GameObject[] silhouetteLeft;
    public GameObject[] silhouetteRight;
    public GameObject monsterVisualCenter;
    public GameObject monsterVisualLeft;
    public GameObject monsterVisualRight;
    [Header("Радар")]
    public RectTransform radarPanel;
    public GameObject radarBlipPrefab;
    [Header("Спрайты")]
    public Sprite type1FarLeft;
    public Sprite type1FarRight;
    public Sprite type1Stroboscope;
    public Sprite type2Attack;
    public Sprite type3Far;
    public Sprite type3Medium;
    public Sprite type3Near;
    public Sprite type3Attack;
    [Header("HP")]
    public int shipHP = 3;
    public UnityEvent onShipHit;
    public UnityEvent onGameOver;
    [Header("Спавн")]
    public float minSpawnInterval = 8f;
    public float maxSpawnInterval = 22f;
    private Monster currentSideMonster;
    private Monster currentCenterMonster;
    private float nextSpawnTime = 0f;
    private float nextSideCheck = 0f;
    private float nextCenterCheck = 0f;
    private void Awake() => Instance = this;
    private void Start()
    {
        nextSpawnTime = Time.time + 2f;
        nextSideCheck = Time.time + 8f;
        nextCenterCheck = Time.time + 25f;
    }
    private void Update()
    {
        if (currentSideMonster != null) currentSideMonster.UpdatePhase(Time.deltaTime);
        if (currentCenterMonster != null) currentCenterMonster.UpdatePhase(Time.deltaTime);
        UpdateVisuals();
        if (Time.time >= nextSideCheck)
        {
            TrySideAttack();
            nextSideCheck = Time.time + 8f;
        }
        if (Time.time >= nextCenterCheck)
        {
            TryCenterAttack();
            nextCenterCheck = Time.time + 30f;
        }
        if (Time.time >= nextSpawnTime)
        {
            SpawnRandomMonster();
            nextSpawnTime = Time.time + Random.Range(minSpawnInterval, maxSpawnInterval);
        }
    }
    private void SpawnRandomMonster()
    {
        int roll = Random.Range(0, 100);
        if (roll < 65)
        {
            bool isLeft = Random.value > 0.5f;
            MonsterType mType = Random.value > 0.5f ? MonsterType.Type1_Visual : MonsterType.Type2_RadarOnly;
            Monster m = CreateMonster();
            m.Reset(mType, isLeft ? Monster.Side.Left : Monster.Side.Right, Random.Range(0.3f, 0.48f));
            if (currentSideMonster != null) Destroy(currentSideMonster.gameObject);
            currentSideMonster = m;
        }
        else
        {
            if (currentCenterMonster == null) currentCenterMonster = CreateMonster();
            currentCenterMonster.Reset(MonsterType.Type3_Central, Monster.Side.Center, 0.28f);
        }
    }
    private Monster CreateMonster()
    {
        GameObject go = new GameObject("Monster");
        go.transform.SetParent(transform);
        Monster m = go.AddComponent<Monster>();
        m.type1FarLeft = type1FarLeft;
        m.type1FarRight = type1FarRight;
        m.type1Stroboscope = type1Stroboscope;
        m.type2Attack = type2Attack;
        m.type3Far = type3Far;
        m.type3Medium = type3Medium;
        m.type3Near = type3Near;
        m.type3Attack = type3Attack;
        return m;
    }
    private void UpdateVisuals()
    {
        bool showDirection = currentSideMonster != null && currentSideMonster.ShouldShowOnMainWindow();
        foreach (var s in silhouetteLeft)  if (s != null) s.SetActive(showDirection && currentSideMonster.side == Monster.Side.Left);
        foreach (var s in silhouetteRight) if (s != null) s.SetActive(showDirection && currentSideMonster.side == Monster.Side.Right);
        bool leftVis = currentSideMonster != null && currentSideMonster.side == Monster.Side.Left && currentSideMonster.IsVisibleOnStroboscope();
        bool rightVis = currentSideMonster != null && currentSideMonster.side == Monster.Side.Right && currentSideMonster.IsVisibleOnStroboscope();
        if (monsterVisualLeft)
        {
            monsterVisualLeft.SetActive(leftVis);
            if (leftVis) ApplySprite(monsterVisualLeft, currentSideMonster.GetCurrentSprite());
        }
        if (monsterVisualRight)
        {
            monsterVisualRight.SetActive(rightVis);
            if (rightVis) ApplySprite(monsterVisualRight, currentSideMonster.GetCurrentSprite());
        }
        if (monsterVisualCenter)
        {
            bool show = currentCenterMonster != null && currentCenterMonster.currentPhase < DistancePhase.Attack;
            monsterVisualCenter.SetActive(show);
            if (show) ApplySprite(monsterVisualCenter, currentCenterMonster.GetCurrentSprite());
        }
        UpdateRadar();
    }
    private void ApplySprite(GameObject obj, Sprite sprite)
    {
        if (obj == null || sprite == null) return;
        Image img = obj.GetComponent<Image>();
        if (img != null) img.sprite = sprite;
    }
    private void UpdateRadar()
    {
        if (radarPanel == null) return;
        foreach (Transform child in radarPanel) Destroy(child.gameObject);
        if (currentSideMonster != null && currentSideMonster.type == MonsterType.Type2_RadarOnly)
        {
            GameObject blip = Instantiate(radarBlipPrefab, radarPanel);
            RectTransform rt = blip.GetComponent<RectTransform>();
            float closeness = currentSideMonster.currentPhase == DistancePhase.Far ? 0.9f :
                              currentSideMonster.currentPhase == DistancePhase.Medium ? 0.6f :
                              currentSideMonster.currentPhase == DistancePhase.Near ? 0.3f : 0.1f;
            Vector2 pos = currentSideMonster.side == Monster.Side.Left ? new Vector2(-65, 0) : new Vector2(65, 0);
            rt.anchoredPosition = pos * closeness;
        }
    }
    private void TrySideAttack()
    {
        if (currentSideMonster == null || currentSideMonster.currentPhase != DistancePhase.Attack) return;
        ShipHit();
        currentSideMonster = null;
    }
    private void TryCenterAttack()
    {
        if (currentCenterMonster == null || currentCenterMonster.currentPhase != DistancePhase.Attack) return;
        LevelProgressManager.Instance?.RollbackProgress(10f);
        ShipHit();
        currentCenterMonster = null;
    }
    private void ShipHit()
    {
        shipHP--;
        onShipHit?.Invoke();
        if (shipHP <= 0) onGameOver?.Invoke();
    }
    public void RepelWithLight()
    {
        if (currentSideMonster != null)
        {
            if (currentSideMonster.type == MonsterType.Type1_Visual) currentSideMonster = null;
            else ShipHit();
        }
    }
    public void RepelWithSound()
    {
        if (currentSideMonster != null)
        {
            if (currentSideMonster.type == MonsterType.Type2_RadarOnly) currentSideMonster = null;
            else ShipHit();
        }
    }
    public void RepelCenter()
    {
        if (currentCenterMonster != null) currentCenterMonster = null;
    }
}