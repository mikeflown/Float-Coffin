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
        UpdateVisuals(false);
    }
    private void TryTurnOn()
    {
        if (EnergyManager.Instance == null) return;
        bool success = EnergyManager.Instance.TryToggleSystem(systemName, true);
        if (success)
            UpdateVisuals(true);
    }
    private void TryTurnOff()
    {
        if (EnergyManager.Instance == null) return;
        bool success = EnergyManager.Instance.TryToggleSystem(systemName, false);
        if (success)
            UpdateVisuals(false);
    }
    private void UpdateVisuals(bool isOn)
    {
        if (offButton)  offButton.gameObject.SetActive(!isOn);
        if (onButton)   onButton.gameObject.SetActive(isOn);
        if (powerIcon)  powerIcon.SetActive(isOn);
    }
}