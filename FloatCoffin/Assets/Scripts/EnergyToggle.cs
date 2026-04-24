using UnityEngine;
using UnityEngine.UI;

public class EnergyToggle : MonoBehaviour
{
    public string systemName;
    public Image background;
    public Toggle toggle;
    private void Start()
    {
        if (toggle == null) toggle = GetComponent<Toggle>();
        toggle.onValueChanged.AddListener(OnToggleChanged);
    }
    private void OnToggleChanged(bool isOn)
    {
        if (EnergyManager.Instance == null) return;
        bool success = EnergyManager.Instance.TryToggleSystem(systemName, isOn);
        if (!success)
        {
            toggle.SetIsOnWithoutNotify(!isOn);
        }
        else
        {
            if (background)
                background.color = isOn ? Color.green : Color.gray;
        }
    }
}