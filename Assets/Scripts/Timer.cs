using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
    [Header("Timer Settings")]
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] float remainingTime = 300f; // 5 minutes (300 seconds)
    
    [Header("UI Panels")]
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] GameObject gameWinPanel; // NEW: Panel for winning
    
    [Header("Game Win Settings")]
    [SerializeField] string winMessage = "Victory!";
    [SerializeField] TextMeshProUGUI winMessageText; // Optional: Text to display win message and time
    
    private bool isGameOver = false;
    private bool isGameWon = false;
    private float elapsedTime = 0f; // Track how much time passed

    void Start()
    {
        if (remainingTime == 0)
            remainingTime = 300f; // Default to 5 minutes if not set
        
        // Hide panels at start
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
            
        if (gameWinPanel != null)
            gameWinPanel.SetActive(false);
    }

    void Update()
    {
        // Only run timer if game is active
        if (!isGameOver && !isGameWon)
        {
            if (remainingTime > 0)
            {
                remainingTime -= Time.deltaTime;
                elapsedTime += Time.deltaTime; // Track elapsed time for win screen
                
                int minutes = Mathf.FloorToInt(remainingTime / 60);
                int seconds = Mathf.FloorToInt(remainingTime % 60);
                timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
            }
            else
            {
                remainingTime = 0;
                timerText.text = "00:00";
                GameOver(); // Time ran out - player loses
            }
        }
    }

    // Called when player dies OR time runs out
    public void GameOver()
    {
        if (isGameOver || isGameWon) return; // Prevent multiple calls
        
        isGameOver = true;
        Time.timeScale = 0f; // Pause the game
        
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
            Debug.Log("Game Over!");
        }
        else
        {
            Debug.LogError("Game Over Panel is not assigned in Timer script!");
        }
    }

    // Called when monster is defeated - player wins!
    public void GameWin()
    {
        if (isGameOver || isGameWon) return; // Prevent multiple calls
        
        isGameWon = true;
        Time.timeScale = 0f; // Pause the game
        
        if (gameWinPanel != null)
        {
            gameWinPanel.SetActive(true);
            
            // Display win message with completion time
            if (winMessageText != null)
            {
                int minutes = Mathf.FloorToInt(elapsedTime / 60f);
                int seconds = Mathf.FloorToInt(elapsedTime % 60f);
                winMessageText.text = $"{winMessage}\nCompleted in: {minutes:00}:{seconds:00}";
            }
            
            Debug.Log("You Win! Monster defeated!");
        }
        else
        {
            Debug.LogError("Game Win Panel is not assigned in Timer script!");
        }
    }

    // Restart the game
    public void RestartGame()
    {
        Time.timeScale = 1f; // Resume FIRST before loading scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
    // Optional: Quit to main menu
    public void QuitToMainMenu()
    {
        Time.timeScale = 1f; // Resume time
        SceneManager.LoadScene("MainMenu"); // Make sure you have a scene named "MainMenu"
    }
}