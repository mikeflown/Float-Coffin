using UnityEngine;
using UnityEngine.UI;

public class SystemPowerToggle : MonoBehaviour
{
    public string systemName;
    public Button offButton;
    public Button onButton;
    public GameObject powerIcon;
    private void Start()
    {
        if (offButton) offButton.onClick.AddListener(TryTurnOn);
        if (onButton)  onButton.onClick.AddListener(TryTurnOff);
        bool isCurrentlyOn = GetCurrentState();
        UpdateVisuals(isCurrentlyOn);
    }
    private bool GetCurrentState()
    {
        if (EnergyManager.Instance == null) return false;
        switch (systemName)
        {
            case "Oxygen": return EnergyManager.Instance.oxygenOn;
            case "Engine": return EnergyManager.Instance.engineOn;
            case "Radar": return EnergyManager.Instance.radarOn;
            case "StrobeLeft": return EnergyManager.Instance.strobeLeftOn;
            case "StrobeRight": return EnergyManager.Instance.strobeRightOn;
            default: return false;
        }
    }
    private void TryTurnOn()
    {
        if (EnergyManager.Instance == null) return;
        bool success = EnergyManager.Instance.TryToggleSystem(systemName, true);
        if (success) UpdateVisuals(true);
    }
    private void TryTurnOff()
    {
        if (EnergyManager.Instance == null) return;

        bool success = EnergyManager.Instance.TryToggleSystem(systemName, false);
        if (success) UpdateVisuals(false);
    }
    private void UpdateVisuals(bool isOn)
    {
        if (offButton)  offButton.gameObject.SetActive(!isOn);
        if (onButton)   onButton.gameObject.SetActive(isOn);
        if (powerIcon)  powerIcon.SetActive(isOn);
    }
}