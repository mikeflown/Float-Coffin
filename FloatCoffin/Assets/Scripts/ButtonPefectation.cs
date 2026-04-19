using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class ButtonSoundAndHighlight : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    [Header("Звуки")]
    public AudioSource audioSource;
    public AudioClip hoverSound;
    public AudioClip clickSound;
    [Header("Подсветка по границам")]
    public Outline outline;
    [Header("Цвет текста")]
    public Color normalTextColor = Color.white;
    public Color hoverTextColor = Color.yellow;
    private TextMeshProUGUI buttonText;
    private void Awake()
    {
        buttonText = GetComponentInChildren<TextMeshProUGUI>();
        if (outline == null)
            outline = GetComponent<Outline>() ?? GetComponentInParent<Outline>();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (audioSource && hoverSound)
            audioSource.PlayOneShot(hoverSound);

        if (outline) outline.enabled = true;
        if (buttonText) buttonText.color = hoverTextColor;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (outline) outline.enabled = false;

        if (buttonText) buttonText.color = normalTextColor;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (audioSource && clickSound)
            audioSource.PlayOneShot(clickSound);
    }
}