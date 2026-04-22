using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    public Image fillImage;
    public float maxHP = 3f;
    private void Start()
    {
        if (SubmarineHealth.Instance != null)
        {
            SubmarineHealth.Instance.onDamaged.AddListener(UpdateHealthBar);
            UpdateHealthBar();
        }
    }
    private void UpdateHealthBar()
    {
        if (SubmarineHealth.Instance == null || fillImage == null) return;
        float current = SubmarineHealth.Instance.currentHP;
        fillImage.fillAmount = current / maxHP;
    }
}