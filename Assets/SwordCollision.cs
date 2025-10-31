using UnityEngine;
using System.Collections.Generic;

public class SwordCollision : MonoBehaviour
{
    [Header("Damage Settings")]
    public float damageAmount = 1f;
    
    [Header("Hit Detection")]
    public LayerMask enemyLayer; // Layer for enemies/monsters
    
    private bool canDamage = false; // Only damage during attack animation
    private HashSet<Collider> hitEnemies = new HashSet<Collider>(); // Track hits to prevent multiple damage per swing
    
    void Start()
    {
        // Make sure the sword collider is a trigger
        Collider col = GetComponent<Collider>();
        if (col != null)
        {
            col.isTrigger = true;
        }
        else
        {
            Debug.LogError("SwordCollision needs a Collider component!");
        }
    }
    
    // Called by PlayerAttack when attack starts
    public void EnableDamage()
    {
        canDamage = true;
        hitEnemies.Clear(); // Reset hit tracking for new swing
        Debug.Log("Sword damage enabled!");
    }
    
    // Called by PlayerAttack when attack ends
    public void DisableDamage()
    {
        canDamage = false;
        Debug.Log("Sword damage disabled!");
    }
    
    void OnTriggerEnter(Collider other)
    {
        // Only damage if we're in attack animation and haven't hit this enemy yet
        if (!canDamage)
        {
            Debug.Log("Sword hit something but damage is disabled.");
            return;
        }
        
        if (hitEnemies.Contains(other))
        {
            Debug.Log("Already hit this enemy in this swing.");
            return;
        }
        
        Debug.Log($"Sword collided with: {other.name} on layer: {LayerMask.LayerToName(other.gameObject.layer)}");
        
        // Check if the object is on the enemy layer
        if (((1 << other.gameObject.layer) & enemyLayer) != 0)
        {
            Debug.Log($"<color=yellow>Sword hit ENEMY: {other.name}</color>");
            
            // Try to damage the monster
            MonsterAIfight monster = other.GetComponent<MonsterAIfight>();
            if (monster != null)
            {
                monster.TakeDamage(damageAmount);
                hitEnemies.Add(other); // Mark as hit to prevent multiple damage
                Debug.Log($"<color=green>Sword dealt {damageAmount} damage to {other.name}!</color>");
            }
            else
            {
                Debug.LogWarning($"Enemy {other.name} doesn't have MonsterAIfight component!");
            }
        }
        else
        {
            Debug.Log($"Object {other.name} is not on enemy layer. Current layer: {LayerMask.LayerToName(other.gameObject.layer)}");
        }
    }
    
    // Also check OnTriggerStay in case OnTriggerEnter is missed
    void OnTriggerStay(Collider other)
    {
        // Only process if we can damage and haven't hit this enemy yet
        if (!canDamage || hitEnemies.Contains(other))
            return;
        
        // Same logic as OnTriggerEnter
        if (((1 << other.gameObject.layer) & enemyLayer) != 0)
        {
            MonsterAIfight monster = other.GetComponent<MonsterAIfight>();
            if (monster != null)
            {
                monster.TakeDamage(damageAmount);
                hitEnemies.Add(other);
                Debug.Log($"<color=green>[TriggerStay] Sword dealt {damageAmount} damage to {other.name}!</color>");
            }
        }
    }
    
    // Optional: Visualize the sword collider in the editor
    void OnDrawGizmos()
    {
        Collider col = GetComponent<Collider>();
        if (col != null)
        {
            Gizmos.color = canDamage ? Color.red : Color.gray;
            
            // Draw based on collider type
            if (col is BoxCollider)
            {
                BoxCollider box = col as BoxCollider;
                Gizmos.matrix = transform.localToWorldMatrix;
                Gizmos.DrawWireCube(box.center, box.size);
            }
            else if (col is CapsuleCollider)
            {
                CapsuleCollider capsule = col as CapsuleCollider;
                Gizmos.matrix = transform.localToWorldMatrix;
                Gizmos.DrawWireSphere(capsule.center, capsule.radius);
            }
        }
    }
}