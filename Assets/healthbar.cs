using UnityEngine;
using System.Collections.Generic;

public class healthbar : MonoBehaviour
{
    public GameObject heartprefab;
    public PlayerStats playerHealth;
    public float health, maxhealth;
    List<healtheart> hearts = new List<healtheart>();

    private void OnEnable()
    {
        // Use the existing callback in PlayerStats
        if (playerHealth != null)
        {
            playerHealth.onHealthChangedCallback += DrawHearts;
        }
    }

    private void OnDisable()
    {
        if (playerHealth != null)
        {
            playerHealth.onHealthChangedCallback -= DrawHearts;
        }
    }

    private void Start()
    {
        DrawHearts();
    }

    public void DrawHearts()
    {
        ClearHearts();
        float maxhealthRemainder = playerHealth.MaxHealth % 2;
        int heartsToMake = (int)(playerHealth.MaxHealth / 2) + (int)maxhealthRemainder;
        for (int i = 0; i < heartsToMake; i++)
        {
            CreateEmptyHeart();
        }

        // Update heart display based on current health
        for (int i = 0; i < hearts.Count; i++)
        {
            int heartStatusRemainder = (int)Mathf.Clamp(playerHealth.Health - (i * 2), 0, 2);
            hearts[i].SetHeartStatus((HeartStatus)heartStatusRemainder); // Changed to SetHeartStatus
        }
    }

    public void CreateEmptyHeart()
    {
        GameObject newHeart = Instantiate(heartprefab);
        newHeart.transform.SetParent(transform);

        healtheart heartComponent = newHeart.GetComponent<healtheart>();
        heartComponent.SetHeartStatus(HeartStatus.empty);
        hearts.Add(heartComponent);
    }

    public void ClearHearts()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        hearts = new List<healtheart>();
    }
}