using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnergyUI : MonoBehaviour
{
    [Header("Слайдеры")]
    public Slider oxygenSlider;
    public Slider strobeLeftSlider;
    public Slider strobeRightSlider;
    public Slider radarSlider;
    public Slider engineSlider;

    [Header("Текст текущей энергии")]
    public TextMeshProUGUI powerText;

    private void Start()
    {
        oxygenSlider.onValueChanged.AddListener(UpdateDistribution);
        strobeLeftSlider.onValueChanged.AddListener(UpdateDistribution);
        strobeRightSlider.onValueChanged.AddListener(UpdateDistribution);
        radarSlider.onValueChanged.AddListener(UpdateDistribution);
        engineSlider.onValueChanged.AddListener(UpdateDistribution);
        UpdateUI();
    }
    private void Update()
    {
        if (powerText && EnergyManager.Instance)
            powerText.text = $"ЭНЕРГИЯ: {EnergyManager.Instance.currentPower} / {EnergyManager.Instance.maxPower}";
    }
    private void UpdateDistribution(float value)
    {
        if (EnergyManager.Instance == null) return;
        EnergyManager.Instance.UpdateDistribution(
            (int)oxygenSlider.value,
            (int)strobeLeftSlider.value,
            (int)strobeRightSlider.value,
            (int)radarSlider.value,
            (int)engineSlider.value
        );
    }
    private void UpdateUI()
    {
        if (EnergyManager.Instance == null) return;
        oxygenSlider.value = EnergyManager.Instance.oxygen;
        strobeLeftSlider.value = EnergyManager.Instance.strobeLeft;
        strobeRightSlider.value = EnergyManager.Instance.strobeRight;
        radarSlider.value = EnergyManager.Instance.radar;
        engineSlider.value = EnergyManager.Instance.engine;
    }
}