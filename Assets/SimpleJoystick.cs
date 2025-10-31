using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SimpleJoystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    [Header("Joystick Settings")]
    public float handleRange = 1f;
    public float deadZone = 0f;

    [Header("Components")]
    public RectTransform background;
    public RectTransform handle;

    private Vector2 input = Vector2.zero;
    private Canvas canvas;
    private Camera cam;

    public float Horizontal => input.x;
    public float Vertical => input.y;
    public Vector2 Direction => input;

    void Start()
    {
        canvas = GetComponentInParent<Canvas>();
        if (canvas == null)
            canvas = GetComponent<Canvas>();

        // Setup camera for screen space overlay
        if (canvas.renderMode == RenderMode.ScreenSpaceCamera)
            cam = canvas.worldCamera;

        // Set default references if not assigned
        if (background == null)
            background = GetComponent<RectTransform>();

        if (handle == null && transform.childCount > 0)
            handle = transform.GetChild(0).GetComponent<RectTransform>();
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 position;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            background,
            eventData.position,
            cam,
            out position))
        {
            // Normalize position
            position.x = (position.x / background.sizeDelta.x);
            position.y = (position.y / background.sizeDelta.y);

            float x = (position.x * 2) - 1;
            float y = (position.y * 2) - 1;

            input = new Vector2(x, y);
            input = (input.magnitude > deadZone) ? input : Vector2.zero;

            // Clamp the input to the handle range
            if (input.magnitude > handleRange)
                input = input.normalized * handleRange;

            // Move the handle
            if (handle != null)
            {
                handle.anchoredPosition = new Vector2(
                    input.x * (background.sizeDelta.x / 2),
                    input.y * (background.sizeDelta.y / 2)
                );
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        input = Vector2.zero;
        if (handle != null)
            handle.anchoredPosition = Vector2.zero;
    }

    public Vector2 GetInputDirection()
    {
        return input;
    }
}