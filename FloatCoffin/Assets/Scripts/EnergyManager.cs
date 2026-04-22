using UnityEngine;

public class EnergyManager : MonoBehaviour
{
    public static EnergyManager Instance { get; private set; }
    [Header("Настройки генератора")]
    public int maxPower = 5;
    public int currentPower = 5;
    [Header("Распределение (изменяется в UI)")]
    public int oxygen = 1;
    public int strobeLeft = 1;
    public int strobeRight = 1;
    public int radar = 1;
    public int engine = 2;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    public bool CanActivateSystem(int cost)
    {
        return currentPower >= cost;
    }
    public void UseEnergy(int amount)
    {
        currentPower = Mathf.Max(0, currentPower - amount);
    }
    public void ReturnEnergy(int amount)
    {
        currentPower = Mathf.Min(maxPower, currentPower + amount);
    }
    public void UpdateDistribution(int newOxygen, int newStrobeL, int newStrobeR, int newRadar, int newEngine)
    {
        oxygen = newOxygen;
        strobeLeft = newStrobeL;
        strobeRight = newStrobeR;
        radar = newRadar;
        engine = newEngine;
    }
}