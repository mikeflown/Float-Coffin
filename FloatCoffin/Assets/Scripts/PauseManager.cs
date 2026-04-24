using UnityEngine;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    public GameObject pausePanel;
    public Text pauseText;
    private bool isPaused = false;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TogglePause();
        }
    }
    public void TogglePause()
    {
        isPaused = !isPaused;
        if (isPaused)
        {
            Time.timeScale = 0f; 
            if (pausePanel) pausePanel.SetActive(true);
            if (pauseText) pauseText.text = "ПАУЗА";
        }
        else
        {
            Time.timeScale = 1f;
            if (pausePanel) pausePanel.SetActive(false);
        }
    }
}