using UnityEngine;

public class LevelComplete : MonoBehaviour
{
    public static void UnlockNextLevel(int completedLevel)
    {
        if (completedLevel == 1)
        {
            PlayerPrefs.SetInt("Level2Unlocked", 1);
        }
        else if (completedLevel == 2)
        {
            PlayerPrefs.SetInt("Level3Unlocked", 1);
        }
        
        PlayerPrefs.Save();
    }
}