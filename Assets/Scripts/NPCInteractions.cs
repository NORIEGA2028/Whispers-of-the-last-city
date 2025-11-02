using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCInteractions : MonoBehaviour
{
    bool player_detection = false;

    void Update()
    {
        // Support keyboard for PC testing
        if (player_detection && Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player_detection = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        player_detection = false;
    }

    public void Interact()
    {
        if (player_detection)
        {
            Debug.Log("Interaction successful!");

        }
    }
}
