using UnityEngine;

public class MobileCameraLook : MonoBehaviour
{
    public Transform playerBody;  // Assign your Player (the body that rotates left/right)
    public float sensitivity = 0.1f;
    public float verticalClamp = 80f;

    private float xRotation = 0f;
    private int lookFingerId = -1;

    void Update()
    {
        // Track touch
        if (Input.touchCount > 0)
        {
            foreach (Touch touch in Input.touches)
            {
                if (touch.phase == TouchPhase.Began && touch.position.x > Screen.width / 2)
                {
                    lookFingerId = touch.fingerId;
                }

                if (touch.fingerId == lookFingerId)
                {
                    if (touch.phase == TouchPhase.Moved)
                    {
                        Vector2 delta = touch.deltaPosition * sensitivity;
                        
                        // Rotate horizontally
                        playerBody.Rotate(Vector3.up * delta.x);

                        // Rotate vertically (camera)
                        xRotation -= delta.y;
                        xRotation = Mathf.Clamp(xRotation, -verticalClamp, verticalClamp);
                        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
                    }

                    if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                    {
                        lookFingerId = -1;
                    }
                }
            }
        }
    }
}