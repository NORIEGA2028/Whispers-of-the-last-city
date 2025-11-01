using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class VideoCutscene : MonoBehaviour
{
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private string nextSceneName = "Level 1";

    void Start()
    {
        // When the video finishes, this event will call OnVideoEnd
        videoPlayer.loopPointReached += OnVideoEnd;

        // Start playing automatically
        videoPlayer.Play();
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        // Load Level 1 after the cutscene ends
        SceneManager.LoadScene(nextSceneName);
    }

    void Update()
    {
        // Optional: skip cutscene by pressing any key
        if (Input.anyKeyDown)
        {
            videoPlayer.Stop();
            SceneManager.LoadScene(nextSceneName);
        }
    }
}