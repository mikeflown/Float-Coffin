using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainScreenController : MonoBehaviour
{
    [Header("UI элементы")]
    public Slider depthSlider;
    public TextMeshProUGUI depthText;
    private void OnEnable()
    {
        if (LevelProgressManager.Instance == null) return;

        LevelProgressManager.Instance.onProgressChanged.AddListener(UpdateDepthUI);
        LevelProgressManager.Instance.onLevelCompleted.AddListener(OnLevelCompleted);
        LevelProgressManager.Instance.onSubmarineBumped.AddListener(OnSubmarineBumped);
    }
    private void OnDisable()
    {
        if (LevelProgressManager.Instance == null) return;
        LevelProgressManager.Instance.onProgressChanged.RemoveListener(UpdateDepthUI);
        LevelProgressManager.Instance.onLevelCompleted.RemoveListener(OnLevelCompleted);
        LevelProgressManager.Instance.onSubmarineBumped.RemoveListener(OnSubmarineBumped);
    }
    private void UpdateDepthUI(float normalized)
    {
        if (depthSlider != null)
            depthSlider.value = normalized;
        if (depthText != null)
        {
            int current = Mathf.RoundToInt(LevelProgressManager.Instance.currentProgress);
            int total = Mathf.RoundToInt(LevelProgressManager.Instance.levelLength);
            depthText.text = $"ГЛУБИНА: {current} / {total}";
        }
    }
    private void OnLevelCompleted()
    {
        Debug.Log("УРОВЕНЬ ПРОЙДЕН");

    }

    private void OnSubmarineBumped(float rollbackAmount)
    {
        Debug.Log($"Монстр №3 ударил! Откат на {rollbackAmount} единиц");
        StartCoroutine(SubmarineBumpAnimation());
    }
    private System.Collections.IEnumerator SubmarineBumpAnimation()
    {
        Transform subContainer = transform;
        Vector3 originalPos = subContainer.localPosition;
        float bumpStrength = 30f;
        subContainer.localPosition = originalPos + Vector3.up * bumpStrength;
        float time = 0f;
        while (time < 0.4f)
        {
            time += Time.deltaTime;
            float t = time / 0.4f;
            float y = Mathf.Lerp(bumpStrength, 0f, t * t);
            subContainer.localPosition = originalPos + Vector3.up * y;
            yield return null;
        }
        subContainer.localPosition = originalPos;
    }
}