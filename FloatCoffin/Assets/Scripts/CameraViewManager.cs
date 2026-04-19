using UnityEngine;

public class CameraViewManager : MonoBehaviour
{
    public enum ViewState { Normal, LeftStrob, RightStrob }
    public ViewState currentState = ViewState.Normal;
    [Header("Наклон камеры при наведении")]
    public Transform cameraPivot;
    public float maxTilt = 8f;
    public float tiltSpeed = 5f;
    [Header("Фоны стробоскопов")]
    public GameObject bgMain;
    public GameObject bgLeftStrob;
    public GameObject bgRightStrob;
    private float targetTilt = 0f;
    private void Update()
    {
        HandleCameraTilt();
        HandleViewSwitch();
    }
    void HandleCameraTilt()
    {
        if (currentState != ViewState.Normal) return;
        float mouseX = Input.mousePosition.x / Screen.width;
        if (mouseX < 0.2f)      targetTilt = maxTilt;      // лево
        else if (mouseX > 0.8f) targetTilt = -maxTilt;     // право
        else                    targetTilt = 0f;
        float current = cameraPivot.localEulerAngles.z;
        if (current > 180f) current -= 360f; // нормализация
        float newTilt = Mathf.LerpAngle(current, targetTilt, Time.deltaTime * tiltSpeed);
        cameraPivot.localRotation = Quaternion.Euler(0, 0, newTilt);
    }
    void HandleViewSwitch()
    {
        if (Input.GetMouseButtonDown(0) && currentState == ViewState.Normal)
        {
            float mouseX = Input.mousePosition.x / Screen.width;
            if (mouseX < 0.25f) SwitchToView(ViewState.LeftStrob);
            else if (mouseX > 0.75f) SwitchToView(ViewState.RightStrob);
        }
        if (Input.GetKeyDown(KeyCode.Escape) || (Input.GetMouseButtonDown(0) && currentState != ViewState.Normal))
        {
            if (currentState != ViewState.Normal)
                SwitchToView(ViewState.Normal);
        }
    }
    public void SwitchToView(ViewState newState)
    {
        currentState = newState;
        bgMain.SetActive(newState == ViewState.Normal);
        bgLeftStrob.SetActive(newState == ViewState.LeftStrob);
        bgRightStrob.SetActive(newState == ViewState.RightStrob);
    }
}