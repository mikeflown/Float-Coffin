using UnityEngine;
using UnityEngine.UI;

public class SubmarineSystems : MonoBehaviour
{
    [Header("Прогресс уровня")]
    public Slider progressBar;
    public float levelLengthMinutes = 8f;
    private float progress = 0f;
    [Header("Энергия")]
    public float maxPower = 100f;
    public float currentPower = 100f;
    public Slider energyBar;
    [Header("Кислород")]
    public Slider oxygenBar;
    public float oxygenConsumptionRate = 5f;
    [Header("Щит (HP)")]
    public Slider shieldBar;
    public int maxHits = 3;
    private int currentHits = 0;
    [Header("Системы, которые можно сломать")]
    public bool headlightsBroken = false;
    public bool soundWaveBroken = false;
    public bool electricShockBroken = false;
    public bool engineBroken = false;
    public bool oxygenGeneratorBroken = false;
    private void Update()
    {
        if (!engineBroken)
            progress += Time.deltaTime / (levelLengthMinutes * 60f);
        progressBar.value = Mathf.Clamp01(progress);
        float depthFactor = Mathf.Lerp(1f, 3f, progress);
        float o2Consumption = oxygenConsumptionRate * depthFactor * Time.deltaTime;
        oxygenBar.value -= o2Consumption;
        if (oxygenBar.value <= 0) { /* Game Over */ }
        currentPower = Mathf.Clamp(currentPower - CalculateEnergyConsumption(), 0, maxPower);
        energyBar.value = currentPower / maxPower;
    }
    float CalculateEnergyConsumption()
    {
        float consumption = 0f;
        if (!headlightsBroken) consumption += 8f;
        if (!soundWaveBroken) consumption += 12f;
        if (!electricShockBroken) consumption += 15f;
        if (!engineBroken) consumption += 20f;
        if (!oxygenGeneratorBroken) consumption += 18f;
        return consumption * Time.deltaTime;
    }
    // Вызывается при ударе монстра 3-го типа
    public void KnockbackSub()
    {
        progress = Mathf.Max(0, progress - 0.08f); // откат назад
        currentHits++;
        shieldBar.value = 1f - (float)currentHits / maxHits;
        if (currentHits >= maxHits) { /* Game Over */ }
    }
}