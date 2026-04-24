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
        UpdateVisuals();
    }
    private void Update()
    {
        UpdateVisuals();
    }
    private void TryTurnOn()
    {
        if (EnergyManager.Instance == null) return;
        bool success = EnergyManager.Instance.TryToggleSystem(systemName, true);
        if (success) UpdateVisuals();
    }
    private void TryTurnOff()
    {
        if (EnergyManager.Instance == null) return;
        bool success = EnergyManager.Instance.TryToggleSystem(systemName, false);
        if (success) UpdateVisuals();
    }
    private void UpdateVisuals()
    {
        if (EnergyManager.Instance == null) return;
        bool isBroken = false;
        bool isOn = false;
        switch (systemName)
        {
            case "Oxygen": isBroken = EnergyManager.Instance.oxygenBroken; isOn = EnergyManager.Instance.oxygenOn; break;
            case "Engine": isBroken = EnergyManager.Instance.engineBroken; isOn = EnergyManager.Instance.engineOn; break;
            case "Radar": isBroken = EnergyManager.Instance.radarBroken; isOn = EnergyManager.Instance.radarOn; break;
            case "StrobeLeft": isBroken = EnergyManager.Instance.strobeLeftBroken; isOn = EnergyManager.Instance.strobeLeftOn; break;
            case "StrobeRight": isBroken = EnergyManager.Instance.strobeRightBroken; isOn = EnergyManager.Instance.strobeRightOn; break;
        }
        if (isBroken)
        {
            if (offButton) offButton.gameObject.SetActive(true);
            if (onButton)  onButton.gameObject.SetActive(false);
            if (powerIcon) powerIcon.SetActive(false);
        }
        else
        {
            if (offButton) offButton.gameObject.SetActive(!isOn);
            if (onButton)  onButton.gameObject.SetActive(isOn);
            if (powerIcon) powerIcon.SetActive(isOn);
        }
    }
}