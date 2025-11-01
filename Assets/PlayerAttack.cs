using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

public class PlayerAttack : MonoBehaviour
{
    [Header("Attack Settings")]
    public float attackRange = 2f;
    public float attackDamage = 1f;
    public float attackCooldown = 0.5f;
    public LayerMask monsterLayer;
    
    [Header("Combat Detection")]
    public float combatDetectionRange = 5f; // Range to detect nearby enemies
    public LayerMask enemyLayer; // Layer for detecting enemies
    
    [Header("Interaction Settings")]
    public float interactionRange = 3f;
    public LayerMask interactableLayer; // Layer for NPCs/interactables
    
    [Header("Visual Feedback")]
    public GameObject attackEffect; // Optional: attack visual effect
    
    [Header("Mobile Support")]
    public MobileInputManager mobileInput; // Reference to mobile input
    
    private float nextAttackTime = 0f;
    private Camera mainCamera;
    private Animator animator;
    private bool isInCombatStance = false;
    private bool isNearEnemy = false;
    
    void Start()
    {
        mainCamera = Camera.main;
        animator = GetComponent<Animator>();
        
        // Try to find mobile input manager if not assigned
        if (mobileInput == null)
        {
            mobileInput = Object.FindFirstObjectByType<MobileInputManager>();
        }
    }
    
    void Update()
    {
        // Check if near enemy for combat stance
        CheckNearbyEnemies();
        
        // Handle attack input from both PC and Mobile
        bool attackInput = GetAttackInput();
        
        if (attackInput && Time.time >= nextAttackTime)
        {
            // Check if there's an interactable nearby first
            if (TryInteract())
            {
                // Interaction happened, don't attack
                return;
            }
            
            // Otherwise, perform attack
            Attack();
            nextAttackTime = Time.time + attackCooldown;
        }
        
        // Handle dedicated interact button on mobile
        if (mobileInput != null && mobileInput.IsInteractPressed())
        {
            TryInteract();
        }
        
        // Update combat stance animation
        UpdateCombatStance();
    }
    
    bool GetAttackInput()
    {
        // Check mobile input first
        if (mobileInput != null && mobileInput.IsAttackPressed())
        {
            return true;
        }
        
        // Check PC input
        #if ENABLE_INPUT_SYSTEM
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            return true;
        }
        #else
        if (Input.GetButtonDown("Fire1"))
        {
            return true;
        }
        #endif
        
        return false;
    }
    
    void CheckNearbyEnemies()
    {
        // Check for enemies within combat detection range
        Collider[] enemies = Physics.OverlapSphere(transform.position, combatDetectionRange, enemyLayer);
        
        bool wasNearEnemy = isNearEnemy;
        isNearEnemy = enemies.Length > 0;
        
        // Log when entering/exiting combat zone
        if (isNearEnemy && !wasNearEnemy)
        {
            Debug.Log("Entered combat zone!");
        }
        else if (!isNearEnemy && wasNearEnemy)
        {
            Debug.Log("Exited combat zone!");
        }
    }
    
    void UpdateCombatStance()
    {
        // Enter combat stance when near enemy
        if (isNearEnemy && !isInCombatStance)
        {
            isInCombatStance = true;
            if (animator != null)
            {
                animator.SetBool("Combat", true);
                Debug.Log("Combat stance enabled");
            }
        }
        // Exit combat stance when no enemies nearby
        else if (!isNearEnemy && isInCombatStance)
        {
            isInCombatStance = false;
            if (animator != null)
            {
                animator.SetBool("Combat", false);
                Debug.Log("Combat stance disabled");
            }
        }
    }
    
    void Attack()
    {
        Debug.Log("Player attacks!");
        
        // Play attack animation
        if (animator != null)
        {
            animator.SetTrigger("Attack 01"); // Matches your animator trigger
        }
        
        // Raycast forward from player
        RaycastHit hit;
        Vector3 rayOrigin = transform.position + Vector3.up; // Start from player center
        Vector3 rayDirection = transform.forward;
        
        if (Physics.Raycast(rayOrigin, rayDirection, out hit, attackRange, monsterLayer))
        {
            Debug.Log("Hit: " + hit.collider.name);
            
            // Check if we hit a monster
            MonsterAIfight monster = hit.collider.GetComponent<MonsterAIfight>();
            if (monster != null)
            {
                monster.TakeDamage(attackDamage);
                Debug.Log("Damaged monster!");
                
                // Optional: Spawn attack effect at hit point
                if (attackEffect != null)
                {
                    Instantiate(attackEffect, hit.point, Quaternion.identity);
                }
            }
        }
        else
        {
            Debug.Log("Attack missed!");
        }
        
        // Visual debug ray
        Debug.DrawRay(rayOrigin, rayDirection * attackRange, Color.red, 0.5f);
    }
    
    bool TryInteract()
    {
        // Check for interactables within range
        Collider[] interactables = Physics.OverlapSphere(transform.position, interactionRange, interactableLayer);
        
        if (interactables.Length > 0)
        {
            // Find the closest interactable
            Collider closest = null;
            float closestDistance = Mathf.Infinity;
            
            foreach (Collider col in interactables)
            {
                float distance = Vector3.Distance(transform.position, col.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closest = col;
                }
            }
            
            if (closest != null)
            {
                // Try to interact with the object
                IInteractable interactable = closest.GetComponent<IInteractable>();
                if (interactable != null)
                {
                    interactable.Interact();
                    Debug.Log("Interacted with: " + closest.name);
                    return true;
                }
                
                // Alternative: Use SendMessage for flexibility
                closest.SendMessage("Interact", SendMessageOptions.DontRequireReceiver);
                Debug.Log("Sent interact message to: " + closest.name);
                return true;
            }
        }
        
        return false;
    }
    
    // Visualize ranges in editor
    void OnDrawGizmosSelected()
    {
        // Attack range
        Gizmos.color = Color.red;
        Vector3 origin = transform.position + Vector3.up;
        Gizmos.DrawRay(origin, transform.forward * attackRange);
        Gizmos.DrawWireSphere(origin + transform.forward * attackRange, 0.2f);
        
        // Combat detection range
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, combatDetectionRange);
        
        // Interaction range
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, interactionRange);
    }
}

// Interface for interactable objects
public interface IInteractable
{
    void Interact();
}