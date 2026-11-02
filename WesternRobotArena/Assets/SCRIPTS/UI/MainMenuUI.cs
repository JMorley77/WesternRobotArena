
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject lobbyPanel;

    public void OpenLobby()
    {
        mainMenuPanel.SetActive(false);
        settingsPanel.SetActive(false);
        lobbyPanel.SetActive(true);
    }

    public void PlayLocal()
    {
        SceneManager.LoadScene("Local");
    }

    public void OpenSettings()
    {
        mainMenuPanel.SetActive(false);
        settingsPanel.SetActive(true);
        lobbyPanel.SetActive(false);
    }

    public void BackToMainMenu()
    {
        mainMenuPanel.SetActive(true);
        settingsPanel.SetActive(false);
        lobbyPanel.SetActive(false);
    }

    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("Exit game requested.");
    }
}
