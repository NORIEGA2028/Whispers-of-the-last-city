using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class CutsceneManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private GameObject darkBackground;
    
    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip voiceoverClip;
    
    [Header("Dialogue Lines")]
    [SerializeField] private DialogueLine[] dialogueLines;
    
    [Header("Settings")]
    [SerializeField] private string nextSceneName = "Level 1";
    [SerializeField] private bool canSkip = true;
    [SerializeField] private KeyCode skipKey = KeyCode.Space;
    
    private int currentLineIndex = 0;
    private bool isPlaying = false;
    
    [System.Serializable]
    public class DialogueLine
    {
        [TextArea(2, 4)]
        public string text;
        public float displayDuration = 3f;
        public AudioClip voiceover;
    }
    
    void Start()
    {
        // Make sure text is empty at start
        if (dialogueText != null)
        {
            dialogueText.text = "";
        }
        
        // Start the cutscene
        StartCoroutine(PlayCutscene());
    }
    
    void Update()
    {
        // Allow skipping if enabled
        if (canSkip && Input.GetKeyDown(skipKey) && isPlaying)
        {
            SkipCutscene();
        }
    }
    
    IEnumerator PlayCutscene()
    {
        isPlaying = true;
        
        // Show dark background
        if (darkBackground != null)
        {
            darkBackground.SetActive(true);
        }
        
        // Play through each dialogue line
        for (currentLineIndex = 0; currentLineIndex < dialogueLines.Length; currentLineIndex++)
        {
            DialogueLine line = dialogueLines[currentLineIndex];
            
            // Display text
            if (dialogueText != null)
            {
                dialogueText.text = line.text;
            }
            
            // Play voiceover if available
            if (line.voiceover != null && audioSource != null)
            {
                audioSource.clip = line.voiceover;
                audioSource.Play();
                
                // Wait for audio to finish or use display duration (whichever is longer)
                float waitTime = Mathf.Max(line.displayDuration, line.voiceover.length);
                yield return new WaitForSeconds(waitTime);
            }
            else
            {
                // No audio, just wait for display duration
                yield return new WaitForSeconds(line.displayDuration);
            }
            
            // Clear text before next line
            if (dialogueText != null)
            {
                dialogueText.text = "";
            }
            
            // Small pause between lines
            yield return new WaitForSeconds(0.5f);
        }
        
        // Cutscene finished
        isPlaying = false;
        LoadNextScene();
    }
    
    void SkipCutscene()
    {
        // Stop all coroutines and audio
        StopAllCoroutines();
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
        
        isPlaying = false;
        LoadNextScene();
    }
    
    void LoadNextScene()
    {
        Debug.Log("Cutscene ended, loading: " + nextSceneName);
        SceneManager.LoadScene(nextSceneName);
    }
}