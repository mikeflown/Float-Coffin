using UnityEngine;
using UnityEngine.UI;

public class EnergyUI : MonoBehaviour
{
    public Image fillBar;
    private void Update()
    {
        if (EnergyManager.Instance != null && fillBar != null)
        {
            fillBar.fillAmount = EnergyManager.Instance.GetRemainingEnergyFillAmount();
        }
    }
}