using UnityEngine;
using UnityEngine.UI;

public class MobileInputManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject mobileControlsCanvas;
    public SimpleJoystick movementJoystick;
    public Button sprintButton;
    public Button attackButton;
    public Button interactButton;

    [Header("Input Values")]
    public Vector2 moveInput;
    public bool sprintPressed;
    public bool attackPressed;
    public bool interactPressed;

    [Header("Auto-detect Platform")]
    public bool autoDetectMobile = true;

    private bool isSprinting = false;

    void Start()
    {
        // Auto-enable mobile controls on mobile devices
        if (autoDetectMobile)
        {
#if UNITY_ANDROID || UNITY_IOS
            EnableMobileControls(true);
#else
            EnableMobileControls(false);
#endif
        }

        // Set up button listeners
        if (sprintButton != null)
        {
            sprintButton.onClick.AddListener(OnSprintButton);
        }

        if (attackButton != null)
        {
            attackButton.onClick.AddListener(OnAttackButton);
        }

        if (interactButton != null)
        {
            interactButton.onClick.AddListener(OnInteractButton);
        }
    }

    void Update()
    {
        // Get joystick input
        if (movementJoystick != null)
        {
            moveInput = new Vector2(movementJoystick.Horizontal, movementJoystick.Vertical);
        }

        sprintPressed = isSprinting;
        
        // Reset one-time buttons after frame
        if (attackPressed) attackPressed = false;
        if (interactPressed) interactPressed = false;
    }

    public void EnableMobileControls(bool enable)
    {
        if (mobileControlsCanvas != null)
        {
            mobileControlsCanvas.SetActive(enable);
        }
    }

    void OnSprintButton()
    {
        isSprinting = !isSprinting;
        Debug.Log("Sprint: " + isSprinting);
        
        // Optional: Change button color when sprinting
        if (sprintButton != null)
        {
            ColorBlock colors = sprintButton.colors;
            colors.normalColor = isSprinting ? new Color(1f, 0.8f, 0f) : Color.white;
            sprintButton.colors = colors;
        }
    }

    void OnAttackButton()
    {
        attackPressed = true;
        Debug.Log("Attack pressed!");
    }

    void OnInteractButton()
    {
        interactPressed = true;
        Debug.Log("Interact pressed!");
    }

    // Public methods for external access
    public Vector2 GetMoveInput() => moveInput;
    public bool IsSprinting() => isSprinting;
    public bool IsAttackPressed() => attackPressed;
    public bool IsInteractPressed() => interactPressed;
}