using UnityEngine;
using UnityEngine.SceneManagement;

public class WinSceneUI : MonoBehaviour
{
    public GameObject winPannel;
    public string mainMenuSceneName = "MainMenu";


    public void ReturnToMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(mainMenuSceneName);
    }

    public void Restart()
    {
        SceneManager.LoadScene("Local");
    }
}
