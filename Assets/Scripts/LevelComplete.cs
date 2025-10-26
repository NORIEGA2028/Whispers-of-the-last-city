using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelComplete : MonoBehaviour
{
    // Call this function when the player completes a level
    public void CompleteLevel(int levelNumber)
    {
        // Unlock the next level
        if (levelNumber == 1)
        {
            PlayerPrefs.SetInt("Level2Unlocked", 1);
            Debug.Log("Level 2 unlocked!");
        }
        else if (levelNumber == 2)
        {
            PlayerPrefs.SetInt("Level3Unlocked", 1);
            Debug.Log("Level 3 unlocked!");
        }
        
        PlayerPrefs.Save();
        
        // Go back to level select
        SceneManager.LoadScene("Level Scene");
    }
    
    // For testing - reset all progress
    public void ResetProgress()
    {
        PlayerPrefs.SetInt("Level2Unlocked", 0);
        PlayerPrefs.SetInt("Level3Unlocked", 0);
        PlayerPrefs.Save();
        Debug.Log("Progress reset!");
    }
}