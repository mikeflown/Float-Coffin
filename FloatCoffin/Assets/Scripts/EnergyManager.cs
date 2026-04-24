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
    [Header("Кислород")]
    public float oxygenLevel = 100f;
    public float oxygenDrainRate = 2f;
    public float oxygenRegenRate = 3f;
    private bool oxygenSystemActive = false;
    private bool hasOxygenDeathTriggered = false;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        usedPower = 0;
        oxygenOn = false;
        engineOn = false;
        radarOn = false;
        strobeLeftOn = false;
        strobeRightOn = false;
    }
    private void Update()
    {
        if (oxygenSystemActive)
        {
            oxygenLevel += oxygenRegenRate * Time.deltaTime;
        }
        else if (oxygenLevel > 0)
        {
            oxygenLevel -= oxygenDrainRate * Time.deltaTime;
        }
        oxygenLevel = Mathf.Clamp(oxygenLevel, 0f, 100f);
        oxygenSystemActive = oxygenOn;
        if (oxygenLevel <= 0 && !hasOxygenDeathTriggered)
        {
            hasOxygenDeathTriggered = true;
            Debug.Log("Смерть от нехватки кислорода!");
            if (DeathScreen.Instance != null)
                DeathScreen.Instance.Show();
        }
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
    public float GetRemainingEnergyFillAmount()
    {
        return 1f - ((float)usedPower / maxPower);
    }
}