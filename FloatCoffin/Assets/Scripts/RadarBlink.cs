using UnityEngine;
using UnityEngine.UI;

public class RadarBlink : MonoBehaviour
{
    private Image image;
    private void Awake()
    {
        image = GetComponent<Image>();
    }
    private void Update()
    {
        if (image != null)
        {
            float alpha = Mathf.PingPong(Time.time * 5f, 1f);
            Color c = image.color;
            c.a = Mathf.Lerp(0.4f, 1f, alpha);
            image.color = c;
        }
    }
}