using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathScreen : MonoBehaviour
{
    public static DeathScreen Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
        gameObject.SetActive(false);
    }
    public void Show()
    {
        gameObject.SetActive(true);
        Time.timeScale = 0f;
    }
    public void RestartLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void QuitGame()
    {
        Debug.Log("Выход из игры...");
        Application.Quit();
    }
}