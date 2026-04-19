using UnityEngine;

public class ScreenManager : MonoBehaviour
{
    [Header("Экраны")]
    public GameObject mainView;
    public GameObject stroboscopsView;
    public GameObject underWaterLeft;
    public GameObject underWaterRight;
    [Header("Дополнительно")]
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
    public void GoToUnderWaterLeft()
    {
        ShowScreen(underWaterLeft);
    }
    public void GoToUnderWaterRight()
    {
        ShowScreen(underWaterRight);
    }
    public void GoBack()
    {
        if (currentScreen == underWaterLeft || currentScreen == underWaterRight)
        {
            ShowScreen(stroboscopsView);
        }
        else if (currentScreen == stroboscopsView)
        {
            ShowScreen(mainView);
        }
    }
    private void ShowScreen(GameObject screenToShow)
    {
        mainView.SetActive(false);
        stroboscopsView.SetActive(false);
        underWaterLeft.SetActive(false);
        underWaterRight.SetActive(false);
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
}