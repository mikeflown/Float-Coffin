using UnityEngine;
using UnityEngine.UI;

public class RepelButton : MonoBehaviour
{
    public enum MonsterType { Type1, Type2, Type3 }
    [Header("Настройки")]
    public MonsterType targetType;
    public MonsterManager.Side targetSide;
    public int requiredPhase = 2;
    public float cooldownTime = 8f;
    [Header("Отображение полоски")]
    [Tooltip("Если true — полоска всегда видна (для центральной кнопки)")]
    public bool alwaysShowCooldownBar = false;
    [Header("UI")]
    public Image cooldownFill;
    public Button button;
    private float cooldownTimer = 0f;
    private bool isOnCooldown = false;
    private void Start()
    {
        if (button == null) button = GetComponent<Button>();
        button.onClick.AddListener(OnButtonPressed);
        if (cooldownFill)
        {
            cooldownFill.fillAmount = 1f;
            cooldownFill.gameObject.SetActive(alwaysShowCooldownBar);
        }
    }
    private void Update()
    {
        if (!isOnCooldown) return;
        if (cooldownFill == null) return;
        cooldownTimer -= Time.deltaTime;
        cooldownFill.fillAmount = (cooldownTime - cooldownTimer) / cooldownTime;
        if (cooldownTimer <= 0f)
        {
            isOnCooldown = false;
            button.interactable = true;
            cooldownFill.fillAmount = 1f;
            if (!alwaysShowCooldownBar)
                cooldownFill.gameObject.SetActive(false);
        }
    }
    private void OnButtonPressed()
    {
        if (isOnCooldown) return;
        bool success = false;
        if (targetType == MonsterType.Type1 && MonsterManager.Instance != null)
            success = MonsterManager.Instance.RepelType1(targetSide);
        else if (targetType == MonsterType.Type2 && MonsterManager.Instance != null)
            success = MonsterManager.Instance.RepelType2(targetSide);
        else if (targetType == MonsterType.Type3 && MonsterManager.Instance != null)
            success = MonsterManager.Instance.RepelType3();
        StartCooldown();
    }
    private void StartCooldown()
    {
        isOnCooldown = true;
        cooldownTimer = cooldownTime;
        button.interactable = false;
        if (cooldownFill)
        {
            cooldownFill.fillAmount = 0f;
            cooldownFill.gameObject.SetActive(true);
        }
    }
}