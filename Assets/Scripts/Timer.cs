using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] float remainingTime = 300f; // 5 minutes (300 seconds)
    [SerializeField] GameObject gameOverPanel;

    void Start()
    {
        if (remainingTime == 0)
            remainingTime = 300f; // Default to 5 minutes if not set
        
        // Hide game over panel at start
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
    }

    void Update()
    {
        if (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
            int minutes = Mathf.FloorToInt(remainingTime / 60);
            int seconds = Mathf.FloorToInt(remainingTime % 60);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
        else
        {
            remainingTime = 0;
            timerText.text = "00:00";
            GameOver();
        }
    }

public void GameOver() // Add "public"
{
    if (gameOverPanel != null)
        gameOverPanel.SetActive(true);
    Time.timeScale = 0f;
}

public void RestartGame()
{
    Time.timeScale = 1f; // Resume FIRST before loading scene
    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
}
}