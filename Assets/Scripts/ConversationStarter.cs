using UnityEngine;
using DialogueEditor;

public class ConversationStarter : MonoBehaviour
{
    [SerializeField] private NPCConversation MotherConversation;
    private bool playerInRange = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

    private void Update()
    {
        // For keyboard testing (PC)
        if (playerInRange && Input.GetKeyDown(KeyCode.F))
        {
            StartConversation();
        }
    }

    // 🔹 This public method will be called by your UI button
    public void StartConversation()
    {
        if (playerInRange)
        {
            ConversationManager.Instance.StartConversation(MotherConversation);
            Debug.Log("Conversation started with Mother NPC!");
        }
    }
}
