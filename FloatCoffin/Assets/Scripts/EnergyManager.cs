using UnityEngine;
using System.Collections;

public class EnergyManager : MonoBehaviour
{
    public static EnergyManager Instance { get; private set; }
    public int maxPower = 5;
    public int usedPower = 0;
    public bool oxygenOn = false;
    public bool engineOn = false;
    public bool radarOn = false;
    public bool strobeLeftOn = false;
    public bool strobeRightOn = false;
    [Header("Кислород")]
    public float oxygenLevel = 100f;
    public float oxygenDrainRate = 2f;
    public float oxygenRegenRate = 3f;
    private bool oxygenSystemActive = false;
    [Header("Поломки")]
    public bool oxygenBroken = false;
    public bool engineBroken = false;
    public bool radarBroken = false;
    public bool strobeLeftBroken = false;
    public bool strobeRightBroken = false;
    [Header("Время ремонта (секунды)")]
    public float oxygenRepairTime = 8f;
    public float engineRepairTime = 12f;
    public float radarRepairTime = 6f;
    public float strobeLeftRepairTime = 7f;
    public float strobeRightRepairTime = 7f;
    [Header("Затраты энергии на ремонт (фиксированные)")]
    public int oxygenRepairCost = 2;
    public int engineRepairCost = 3;
    public int radarRepairCost = 2;
    public int strobeLeftRepairCost = 2;
    public int strobeRightRepairCost = 2;
    [Header("Шансы поломок (%)")]
    public float type1BreakChance = 35f;
    public float type2BreakChance = 38f;
    public float type3BreakChance = 20f;
    public float oxygenFullBreakChance = 10f;
    public float oxygenFullBreakInterval = 4f;
    public float randomBreakChance = 20;
    public float randomBreakInterval =140f;
    private float oxygenFullBreakTimer = 0f;
    private float randomBreakTimer = 0f;
    private bool hasOxygenDeathTriggered = false;
    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        Instance = this;
    }
    private void Update()
    {
        if (oxygenSystemActive)
            oxygenLevel += oxygenRegenRate * Time.deltaTime;
        else if (oxygenLevel > 0)
            oxygenLevel -= oxygenDrainRate * Time.deltaTime;
        oxygenLevel = Mathf.Clamp(oxygenLevel, 0f, 100f);
        oxygenSystemActive = oxygenOn;
        if (oxygenLevel <= 0 && !hasOxygenDeathTriggered)
        {
            hasOxygenDeathTriggered = true;
            if (DeathScreen.Instance) DeathScreen.Instance.Show();
        }
        if (oxygenSystemActive && oxygenLevel >= 99f)
        {
            oxygenFullBreakTimer += Time.deltaTime;
            if (oxygenFullBreakTimer >= oxygenFullBreakInterval)
            {
                oxygenFullBreakTimer = 0f;
                if (Random.Range(0f, 100f) < oxygenFullBreakChance)
                {
                    ForceTurnOffSystem("Oxygen");
                    oxygenBroken = true;
                    Debug.Log("Поломка: Кислородный генератор!");
                }
            }
        }
        randomBreakTimer += Time.deltaTime;
        if (randomBreakTimer >= randomBreakInterval)
        {
            randomBreakTimer = 0f;
            TryRandomBreak();
        }
    }
    public void ForceTurnOffSystem(string systemName)
    {
        switch (systemName)
        {
            case "Oxygen": oxygenOn = false; break;
            case "Engine": engineOn = false; break;
            case "Radar": radarOn = false; break;
            case "StrobeLeft": strobeLeftOn = false; break;
            case "StrobeRight": strobeRightOn = false; break;
        }
        int cost = (systemName == "Engine") ? 2 : 1;
        usedPower -= cost;
        if (usedPower < 0) usedPower = 0;
        Debug.Log($"Система {systemName} принудительно выключена");
    }
    public void TryBreakFromType1(bool isLeft)
    {
        if (Random.Range(0f, 100f) < type1BreakChance)
        {
            if (isLeft)
            {
                ForceTurnOffSystem("StrobeLeft");
                strobeLeftBroken = true;
            }
            else
            {
                ForceTurnOffSystem("StrobeRight");
                strobeRightBroken = true;
            }
            Debug.Log($"Поломка: Стробоскоп {(isLeft ? "L" : "R")}");
        }
    }
    public void TryBreakFromType2()
    {
        if (Random.Range(0f, 100f) < type2BreakChance)
        {
            ForceTurnOffSystem("Radar");
            radarBroken = true;
            Debug.Log("Поломка: Радар");
        }
    }
    public void TryBreakFromType3()
    {
        if (Random.Range(0f, 100f) < type3BreakChance)
        {
            ForceTurnOffSystem("Engine");
            engineBroken = true;
            Debug.Log("Поломка: Двигатель");
        }
    }
    private void TryRandomBreak()
    {
        if (Random.Range(0f, 100f) < randomBreakChance)
        {
            int r = Random.Range(0, 5);
            switch (r)
            {
                case 0: ForceTurnOffSystem("Oxygen"); oxygenBroken = true; Debug.Log("Поломка: Кислород"); break;
                case 1: ForceTurnOffSystem("Engine"); engineBroken = true; Debug.Log("Поломка: Двигатель"); break;
                case 2: ForceTurnOffSystem("Radar"); radarBroken = true; Debug.Log("Поломка: Радар"); break;
                case 3: ForceTurnOffSystem("StrobeLeft"); strobeLeftBroken = true; Debug.Log("Поломка: Строб L"); break;
                case 4: ForceTurnOffSystem("StrobeRight"); strobeRightBroken = true; Debug.Log("Поломка: Строб R"); break;
            }
        }
    }
    public IEnumerator RepairSystem(string systemName)
    {
        bool wasOn = false;
        switch (systemName)
        {
            case "Oxygen": wasOn = oxygenOn; oxygenOn = false; break;
            case "Engine": wasOn = engineOn; engineOn = false; break;
            case "Radar": wasOn = radarOn; radarOn = false; break;
            case "StrobeLeft": wasOn = strobeLeftOn; strobeLeftOn = false; break;
            case "StrobeRight": wasOn = strobeRightOn; strobeRightOn = false; break;
        }
        switch (systemName)
        {
            case "Oxygen": oxygenBroken = false; oxygenOn = wasOn; break;
            case "Engine": engineBroken = false; engineOn = wasOn; break;
            case "Radar": radarBroken = false; radarOn = wasOn; break;
            case "StrobeLeft": strobeLeftBroken = false; strobeLeftOn = wasOn; break;
            case "StrobeRight": strobeRightBroken = false; strobeRightOn = wasOn; break;
        }
        Debug.Log($"Отремонтировано: {systemName}");
        yield break;
    }
    public bool CanRepair(string systemName)
    {
        switch (systemName)
        {
            case "Oxygen": return oxygenBroken;
            case "Engine": return engineBroken;
            case "Radar": return radarBroken;
            case "StrobeLeft": return strobeLeftBroken;
            case "StrobeRight": return strobeRightBroken;
            default: return false;
        }
    }
    public bool TryToggleSystem(string systemName, bool turnOn)
    {
        if (turnOn && IsSystemBroken(systemName)) return false;
        int cost = (systemName == "Engine") ? 2 : 1;
        if (!turnOn)
        {
            usedPower -= cost;
            if (usedPower < 0) usedPower = 0;
        }
        else
        {
            int newUsed = usedPower + cost;
            if (newUsed > maxPower) return false;
            usedPower = newUsed;
        }
        switch (systemName)
        {
            case "Oxygen": oxygenOn = turnOn; break;
            case "Engine": engineOn = turnOn; break;
            case "Radar": radarOn = turnOn; break;
            case "StrobeLeft": strobeLeftOn = turnOn; break;
            case "StrobeRight": strobeRightOn = turnOn; break;
        }
        return true;
    }
    private bool IsSystemBroken(string systemName)
    {
        switch (systemName)
        {
            case "Oxygen": return oxygenBroken;
            case "Engine": return engineBroken;
            case "Radar": return radarBroken;
            case "StrobeLeft": return strobeLeftBroken;
            case "StrobeRight": return strobeRightBroken;
            default: return false;
        }
    }
    public float GetEnergyFillAmount() => (float)usedPower / maxPower;
    public float GetRemainingEnergyFillAmount() => 1f - ((float)usedPower / maxPower);
}