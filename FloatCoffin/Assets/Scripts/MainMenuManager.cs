using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [Header("Панели")]
    public GameObject mainMenuPanel;
    public GameObject settingsPanel;

    private void Start()
    {
        if (mainMenuPanel) mainMenuPanel.SetActive(true);
        if (settingsPanel) settingsPanel.SetActive(false);
    }
    public void PlayGame()
    {
        SceneManager.LoadScene("Gameplay");
        // Или: SceneManager.LoadScene(1); // ну допустим может быть буду использовать
    }
    public void OpenSettings()
    {
        mainMenuPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }
    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }
    public void QuitGame()
    {
        Debug.Log("Игра закрывается...");

        // Это работает в билде и правильно завершает игру в редакторе
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}