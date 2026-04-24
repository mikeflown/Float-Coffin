using UnityEngine;

public class ScreenManager : MonoBehaviour
{
    public GameObject mainView;
    public GameObject stroboscopsView;
    public GameObject underWaterLeft;
    public GameObject underWaterRight;
    public GameObject tabletMenu;
    public GameObject progressPanel;
    private GameObject currentScreen;
    void Start()
    {
        ShowScreen(mainView);
    }
    public void GoToStroboscops()
    {
        ShowScreen(stroboscopsView);
    }
    public void GoToTabletMenu()
    {
        ShowScreen(tabletMenu);
    }
    public void GoToUnderWaterLeft()
    {
        if (EnergyManager.Instance == null || !EnergyManager.Instance.strobeLeftOn)
        {
            Debug.Log("Стробоскоп L выключен! Включи его в планшете энергии.");
            return;
        }
        ShowScreen(underWaterLeft);
    }
    public void GoToUnderWaterRight()
    {
        if (EnergyManager.Instance == null || !EnergyManager.Instance.strobeRightOn)
        {
            Debug.Log("Стробоскоп R выключен! Включи его в планшете энергии.");
            return;
        }
        ShowScreen(underWaterRight);
    }
    public void GoBack()
    {
        if (currentScreen == underWaterLeft || currentScreen == underWaterRight)
        {
            ShowScreen(stroboscopsView);
        }
        else if (currentScreen == stroboscopsView || currentScreen == tabletMenu)
        {
            ShowScreen(mainView);
        }
    }
    private void ShowScreen(GameObject screenToShow)
    {
        if (mainView != null) mainView.SetActive(false);
        if (stroboscopsView != null) stroboscopsView.SetActive(false);
        if (underWaterLeft != null) underWaterLeft.SetActive(false);
        if (underWaterRight != null) underWaterRight.SetActive(false);
        if (tabletMenu != null) tabletMenu.SetActive(false);
        if (screenToShow != null)
        {
            screenToShow.SetActive(true);
            currentScreen = screenToShow;
        }
        if (progressPanel != null)
            progressPanel.SetActive(true);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GoBack();
        }
    }
    public void ReturnToMainView()
    {
        ShowScreen(mainView);
    }
}