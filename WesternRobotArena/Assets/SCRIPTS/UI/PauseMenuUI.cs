
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuUI : MonoBehaviour
{
    public GameObject pauseMenuPanel;
    public string mainMenuSceneName = "MainMenu";

    private LocalInputHandler[] inputHandlers;
    private bool isPaused;

    void Start()
    {
        pauseMenuPanel.SetActive(false);
        Time.timeScale = 1;
        isPaused = false;
    }

    void Update()
    {
        // Find all currently spawned players
        inputHandlers = FindObjectsByType<LocalInputHandler>(FindObjectsSortMode.None);

        foreach (LocalInputHandler handler in inputHandlers)
        {
            if (handler.PausePressed)
            {
                if (isPaused)
                {
                    ResumeGame();
                }
                else
                {
                    PauseGame();
                }
                break;
            }
        }
    }

    public void PauseGame()
    {
        pauseMenuPanel.SetActive(true);
        Time.timeScale = 0;
        isPaused = true;
    }

    public void ResumeGame()
    {
        pauseMenuPanel.SetActive(false);
        Time.timeScale = 1;
        isPaused = false;
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(mainMenuSceneName);
    }
}
