using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement; // Add this for scene reloading

public class Timer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] float remainingTime = 10f; // 3 minutes
    [SerializeField] GameObject gameOverPanel; // Add this - drag your GameOverPanel here

    void Start()
    {
        if (remainingTime == 0)
            remainingTime = 180f;
        
        // Hide game over panel at start
        gameOverPanel.SetActive(false);
    }

    void Update()
    {
        if (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime * 1.2f;
            int minutes = Mathf.FloorToInt(remainingTime / 60);
            int seconds = Mathf.FloorToInt(remainingTime % 60);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
        else
        {
            remainingTime = 0;
            timerText.text = "00:00";
            GameOver(); // Call game over function
        }
    }

    void GameOver()
    {
        gameOverPanel.SetActive(true); // Show the game over panel
        Time.timeScale = 0f; // Pause the game (optional)
    }

    // Function to restart the game
    public void RestartGame()
    {
        Time.timeScale = 1f; // Resume normal time
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reload current scene
    }
}