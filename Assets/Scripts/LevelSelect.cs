using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelect : MonoBehaviour
{
    [SerializeField] private Button chapter1Button;
    [SerializeField] private Button chapter2Button;
    [SerializeField] private Button chapter3Button;
    
    void Start()
    {
        // Chapter 1 is always unlocked
        int level1Unlocked = PlayerPrefs.GetInt("Level1Unlocked", 1); // 1 = unlocked
        int level2Unlocked = PlayerPrefs.GetInt("Level2Unlocked", 0); // 0 = locked
        int level3Unlocked = PlayerPrefs.GetInt("Level3Unlocked", 0);
        
        // Set button interactability
        chapter1Button.interactable = true; // Always unlocked
        chapter2Button.interactable = (level2Unlocked == 1);
        chapter3Button.interactable = (level3Unlocked == 1);
    }
    
    public void LoadLevel1()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Cutscene_Level1");
    }
    
    public void LoadLevel2()
    {
        SceneManager.LoadScene("Level 2");
    }
    
    public void LoadLevel3()
    {
        SceneManager.LoadScene("Level 3");
    }
    
    public void BackToMainMenu()
    {
        SceneManager.LoadScene("Main menu");
    }
}