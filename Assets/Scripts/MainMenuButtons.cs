using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButtons : MonoBehaviour
{
    [SerializeField] private string newGameLevel = "Level 1";
    [SerializeField] private GameObject exitConfirmationPanel;
    [SerializeField] private MainMenuSettings settingsController;

    public void NewGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Level Scene");
    }

    public void ContinueGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(newGameLevel);
    }

    public void Options()
    {
        settingsController.OpenSettings();
    }

    public void ExitGame()
    {
        exitConfirmationPanel.SetActive(true);
    }

    public void ConfirmExit()
    {
        Application.Quit();
        Debug.Log("Game is exiting");
    }

    public void CancelExit()
    {
        exitConfirmationPanel.SetActive(false);
    }
}