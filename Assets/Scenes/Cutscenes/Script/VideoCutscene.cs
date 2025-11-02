using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class VideoCutscene : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public string nextSceneName = "Level1"; 
    public Button skipButton;

    void Start()
    {
        // Play the video automatically
        videoPlayer.loopPointReached += OnVideoEnd;

        // Make sure the Skip button calls SkipCutscene when clicked
        if (skipButton != null)
            skipButton.onClick.AddListener(SkipCutscene);
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        SceneManager.LoadScene(nextSceneName);
    }

    public void SkipCutscene()
    {
        SceneManager.LoadScene(nextSceneName);
    }
}