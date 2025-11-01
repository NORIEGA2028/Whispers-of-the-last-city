using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

[RequireComponent(typeof(NavMeshAgent))]
public class MonsterAIfight : MonoBehaviour
{
    [Header("Target & Combat")]
    public Transform Target;
    public float AttackDistance = 2f;
    public float attackCooldown = 1.5f;
    public float damagePerAttack = 1f;

    [Header("Monster Health")]
    public float monsterHealth = 3f;
    public float monsterMaxHealth = 3f;

    [Header("Game Over Settings")]
    [SerializeField] private float delayBeforeGameOver = 1f;
    [SerializeField] private float delayBeforeDeath = 0.5f;
    
    [Header("Game Win Settings")]
    [SerializeField] private float delayBeforeGameWin = 1.5f; // Delay before showing win screen

    [Header("Settings")]
    public bool useRootMotion = false;

    private NavMeshAgent m_Agent;
    private Animator m_Animator;
    private float m_Distance;
    private float nextAttackTime = 0f;
    private bool playerIsDead = false;
    private bool monsterIsDead = false;
    private Timer timerScript;
    private PlayerStats playerStats;

    void Start()
    {
        // Try to snap monster to NavMesh if not close enough
        NavMeshHit hit;
        if (NavMesh.SamplePosition(transform.position, out hit, 10f, NavMesh.AllAreas))
        {
            transform.position = hit.position;
            Debug.Log("Monster snapped to NavMesh at: " + hit.position);
        }
        else
        {
            Debug.LogError("Monster is too far from NavMesh! Move it closer to the blue area.");
        }

        m_Agent = GetComponent<NavMeshAgent>();
        m_Animator = GetComponent<Animator>();

        // Find the Timer script in the scene
        timerScript = Object.FindFirstObjectByType<Timer>();
        if (timerScript == null)
        {
            Debug.LogWarning("Timer script not found in scene!");
        }

        // Find the PlayerStats script
        if (Target != null)
        {
            playerStats = Target.GetComponent<PlayerStats>();
            if (playerStats == null)
            {
                Debug.LogError("PlayerStats component not found on target!");
            }
        }

        // Disable automatic updates if using root motion
        if (useRootMotion)
        {
            m_Agent.updatePosition = false;
            m_Agent.updateRotation = false;
        }

        if (Target != null)
            Debug.Log("Monster initialized. Target: " + Target.name);
    }

    void Update()
    {
        if (Target == null || playerIsDead || monsterIsDead) return;

        m_Distance = Vector3.Distance(transform.position, Target.position);

        if (m_Distance <= AttackDistance)
        {
            // Close enough to attack
            m_Agent.isStopped = true;

            if (m_Animator != null)
            {
                m_Animator.SetBool("isAttacking", true);
                m_Animator.SetBool("isWalking", false);
            }

            // Face target while attacking
            Vector3 direction = (Target.position - transform.position).normalized;
            direction.y = 0;
            if (direction != Vector3.zero)
            {
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
            }

            // Attack the player
            if (Time.time >= nextAttackTime)
            {
                AttackPlayer();
                nextAttackTime = Time.time + attackCooldown;
            }
        }
        else
        {
            // Too far, chase the target
            m_Agent.isStopped = false;
            m_Agent.SetDestination(Target.position);

            if (m_Animator != null)
            {
                m_Animator.SetBool("isAttacking", false);
                m_Animator.SetBool("isWalking", true);
            }
        }

        // Debug visualization
        Debug.DrawLine(transform.position, Target.position,
            m_Distance <= AttackDistance ? Color.red : Color.green);
    }

    void AttackPlayer()
    {
        Debug.Log("Monster attacks player!");

        if (!playerIsDead && playerStats != null)
        {
            // Deal damage to the player
            playerStats.TakeDamage(damagePerAttack);
            Debug.Log($"Monster dealt {damagePerAttack} damage. Player health: {playerStats.Health}");

            // Check if player died from this attack
            if (playerStats.Health <= 0)
            {
                StartCoroutine(AttackAndTriggerGameOver());
            }
        }
    }

    // Called by player when they attack the monster
    public void TakeDamage(float damage)
    {
        if (monsterIsDead)
        {
            Debug.Log("Monster is already dead, ignoring damage.");
            return;
        }

        monsterHealth -= damage;
        Debug.Log($"<color=red>Monster took {damage} damage! Remaining health: {monsterHealth}/{monsterMaxHealth}</color>");

        // Visual feedback (optional - you can add hurt animation here)
        if (m_Animator != null)
        {
            // You can trigger a hurt animation if you have one
            // m_Animator.SetTrigger("Hurt");
        }

        // Check if monster died
        if (monsterHealth <= 0)
        {
            Die();
        }
    }

    // Alternative: Detect damage through collision (backup method)
    void OnTriggerEnter(Collider other)
    {
        // Check if hit by sword
        if (other.CompareTag("Weapon") || other.GetComponent<SwordCollision>() != null)
        {
            SwordCollision sword = other.GetComponent<SwordCollision>();
            if (sword != null)
            {
                // This is handled by SwordCollision calling TakeDamage
                Debug.Log("Monster detected sword collision!");
            }
        }
    }

    // Handle monster death and trigger game win
    private void Die()
    {
        monsterIsDead = true;
        Debug.Log("Monster has been defeated!");

        // Stop the agent
        if (m_Agent != null)
        {
            m_Agent.isStopped = true;
            m_Agent.enabled = false;
        }

        // Play death animation if you have one
        if (m_Animator != null)
        {
            m_Animator.SetBool("isAttacking", false);
            m_Animator.SetBool("isWalking", false);
            
            // Try to set death trigger/bool - check what parameter your animator uses
            // Option 1: If you have a "Death" trigger
            if (HasParameter(m_Animator, "Death"))
            {
                m_Animator.SetTrigger("Death");
            }
            // Option 2: If you have an "IsDead" bool
            else if (HasParameter(m_Animator, "IsDead"))
            {
                m_Animator.SetBool("IsDead", true);
            }
            // Option 3: If you have a "Dead" bool
            else if (HasParameter(m_Animator, "Dead"))
            {
                m_Animator.SetBool("Dead", true);
            }
            else
            {
                Debug.LogWarning("No death animation parameter found in animator!");
            }
        }

        // Trigger game win sequence
        StartCoroutine(TriggerGameWin());
    }
    
    // Helper method to check if animator has a parameter
    private bool HasParameter(Animator animator, string paramName)
    {
        foreach (AnimatorControllerParameter param in animator.parameters)
        {
            if (param.name == paramName)
                return true;
        }
        return false;
    }

    private IEnumerator TriggerGameWin()
    {
        // Wait for death animation to play
        yield return new WaitForSeconds(delayBeforeGameWin);

        // Trigger game win through Timer script
        if (timerScript != null)
        {
            timerScript.GameWin(); // Call the GameWin method
            Debug.Log("Game Win triggered! Monster defeated!");
        }
        else
        {
            Debug.LogError("Cannot trigger Game Win - Timer script not found!");
        }

        // Destroy the monster after showing win screen
        yield return new WaitForSeconds(delayBeforeDeath);
        Destroy(gameObject);
        Debug.Log("Monster destroyed!");
    }

    private IEnumerator AttackAndTriggerGameOver()
    {
        playerIsDead = true;

        // Play attack animation on monster
        if (m_Animator != null)
            m_Animator.SetBool("isAttacking", true);

        // Trigger death animation on PLAYER
        if (Target != null)
        {
            Animator playerAnimator = Target.GetComponent<Animator>();
            if (playerAnimator != null)
            {
                // Check which death parameter the player animator uses
                if (HasParameter(playerAnimator, "Death"))
                {
                    playerAnimator.SetTrigger("Death");
                    Debug.Log("Player death animation triggered!");
                }
                else if (HasParameter(playerAnimator, "IsDead"))
                {
                    playerAnimator.SetBool("IsDead", true);
                    Debug.Log("Player IsDead set to true!");
                }
                else if (HasParameter(playerAnimator, "Dead"))
                {
                    playerAnimator.SetBool("Dead", true);
                    Debug.Log("Player Dead set to true!");
                }
                else
                {
                    Debug.LogWarning("No death parameter found in player animator!");
                }
            }
        }

        // Wait before showing game over
        yield return new WaitForSeconds(delayBeforeGameOver);

        // Trigger game over through Timer script
        if (timerScript != null)
        {
            timerScript.GameOver();
            Debug.Log("Game Over triggered by Monster!");
        }
        else
        {
            Debug.LogError("Cannot trigger Game Over - Timer script not found!");
        }
    }

    void OnAnimatorMove()
    {
        // Only use this if root motion is enabled
        if (!useRootMotion) return;

        // Sync the agent's position with the animator's root motion
        Vector3 position = m_Animator.rootPosition;
        position.y = m_Agent.nextPosition.y; // Keep NavMesh Y position
        transform.position = position;

        // Update the NavMeshAgent so it knows where we are
        m_Agent.nextPosition = transform.position;
    }

    void OnDrawGizmosSelected()
    {
        // Show attack range
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, AttackDistance);

        if (Target != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, Target.position);
        }
    }
}