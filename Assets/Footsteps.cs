using UnityEngine;

public class Footsteps : MonoBehaviour
{
    [Header("Audio Settings")]
    public AudioSource footstepAudioSource;
    public AudioClip[] footstepSounds;
    
    [Header("Movement Detection")]
    public float stepInterval = 0.5f; // Time between footsteps
    
    private float stepTimer = 0f;
    private Vector3 lastPosition;
    private bool isMoving = false;

    void Start()
    {
        lastPosition = transform.position;
        
        if (footstepAudioSource == null)
        {
            footstepAudioSource = GetComponent<AudioSource>();
        }
    }

    void Update()
    {
        // Check if the mob is moving
        isMoving = Vector3.Distance(transform.position, lastPosition) > 0.01f;
        
        if (isMoving)
        {
            stepTimer += Time.deltaTime;
            
            // Play footstep sound at intervals
            if (stepTimer >= stepInterval)
            {
                PlayFootstepSound();
                stepTimer = 0f;
            }
        }
        else
        {
            // Reset timer when not moving
            stepTimer = 0f;
        }
        
        lastPosition = transform.position;
    }

    void PlayFootstepSound()
    {
        if (footstepAudioSource != null && footstepSounds.Length > 0)
        {
            // Play a random footstep sound from the array
            AudioClip clip = footstepSounds[Random.Range(0, footstepSounds.Length)];
            footstepAudioSource.PlayOneShot(clip);
        }
    }
}