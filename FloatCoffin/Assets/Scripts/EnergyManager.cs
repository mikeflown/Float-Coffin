using UnityEngine;

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
    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        Instance = this;
    }
    public bool TryToggleSystem(string systemName, bool turnOn)
    {
        int cost = (systemName == "Engine") ? 2 : 1;
        int newUsed = usedPower;
        if (turnOn)
            newUsed += cost;
        else
            newUsed -= cost;
        if (newUsed > maxPower || newUsed < 0)
            return false;
        usedPower = newUsed;
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
    public float GetEnergyFillAmount()
    {
        return (float)usedPower / maxPower;
    }
}