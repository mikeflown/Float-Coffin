using UnityEngine;
using UnityEngine.UI;

public class OxygenUI : MonoBehaviour
{
    [Header("Кислород")]
    public Image oxygenBar;
    public GameObject orangeWarning;
    public GameObject redWarning;
    [Header("Иконка планшета (с сохранением hover)")]
    public Button tabletButton;
    public Image tabletImage;
    [Header("Нормальные спрайты")]
    public Sprite normalOff;
    public Sprite normalOn;
    [Header("Спрайты предупреждения")]
    public Sprite warningOff;
    public Sprite warningOn;
    private void Update()
    {
        if (EnergyManager.Instance == null || tabletButton == null) return;
        float oxygen = EnergyManager.Instance.oxygenLevel;
        if (oxygenBar) oxygenBar.fillAmount = oxygen / 100f;
        if (orangeWarning) orangeWarning.SetActive(oxygen < 50 && oxygen >= 20);
        if (redWarning)    redWarning.SetActive(oxygen < 20);
        bool lowOxygen = oxygen < 35f;
        if (tabletImage)
        {
            tabletImage.sprite = lowOxygen ? warningOff : normalOff;
        }
        var colors = tabletButton.spriteState;
        colors.highlightedSprite = lowOxygen ? warningOn : normalOn;
        colors.pressedSprite = lowOxygen ? warningOn : normalOn;
        tabletButton.spriteState = colors;
    }
}