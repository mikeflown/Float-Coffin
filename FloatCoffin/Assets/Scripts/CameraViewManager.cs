using UnityEngine;

public class CameraViewManager : MonoBehaviour
{
    [Header("Фоны")]
    public RectTransform mainBackground;           // Центральный интерьер
    public GameObject leftStrobBackground;         // Левый подводный мир
    public GameObject rightStrobBackground;        // Правый подводный мир

    [Header("UI")]
    public GameObject strobButtonsPanel;

    [Header("Настройки")]
    public float maxShift = 520f;                  // Сила сдвига главного фона
    public float panSpeed = 16f;
    public float edgeThreshold = 0.18f;            // Зона активации

    [Header("Автопереход в стробоскоп")]
    public float enterThreshold = 380f;            // При каком сдвиге автоматически входить
    public float exitThreshold = 100f;             // При каком сдвиге возвращаться назад

    private float currentShift = 0f;
    private enum Mode { Center, Left, Right }
    private Mode currentMode = Mode.Center;

    void Start()
    {
        ShowCenter();
    }

    void Update()
    {
        if (currentMode == Mode.Center)
        {
            HandlePanning();
            HandleAutoEnter();
        }
        else
        {
            HandleAutoExit();
            if (Input.GetKeyDown(KeyCode.Escape))
                ShowCenter();
        }
    }

    void HandlePanning()
    {
        float mouseX = Input.mousePosition.x / Screen.width;

        float target = 0f;
        if (mouseX < edgeThreshold)        target = maxShift;     // мышь влево → фон вправо
        else if (mouseX > 1f - edgeThreshold) target = -maxShift; // мышь вправо → фон влево

        currentShift = Mathf.Lerp(currentShift, target, Time.deltaTime * panSpeed);

        Vector2 pos = mainBackground.anchoredPosition;
        pos.x = currentShift;
        mainBackground.anchoredPosition = pos;
    }

    void HandleAutoEnter()
    {
        if (currentShift > enterThreshold)
            EnterStrobMode(Mode.Left);
        else if (currentShift < -enterThreshold)
            EnterStrobMode(Mode.Right);
    }

    void HandleAutoExit()
    {
        // Если игрок отводит мышку назад — плавно возвращаемся
        if (currentMode == Mode.Left && currentShift < exitThreshold)
            ShowCenter();
        else if (currentMode == Mode.Right && currentShift > -exitThreshold)
            ShowCenter();
    }

    void EnterStrobMode(Mode mode)
    {
        currentMode = mode;

        mainBackground.gameObject.SetActive(false);

        leftStrobBackground.SetActive(mode == Mode.Left);
        rightStrobBackground.SetActive(mode == Mode.Right);

        strobButtonsPanel.SetActive(true);

        mainBackground.anchoredPosition = Vector2.zero;
        currentShift = 0f;
    }

    public void ShowCenter()
    {
        currentMode = Mode.Center;
        currentShift = 0f;

        mainBackground.gameObject.SetActive(true);
        leftStrobBackground.SetActive(false);
        rightStrobBackground.SetActive(false);
        strobButtonsPanel.SetActive(false);

        mainBackground.anchoredPosition = Vector2.zero;
    }
}