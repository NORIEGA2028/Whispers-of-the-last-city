using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class healtheart : MonoBehaviour
{
    public Sprite Fullheart, Emptyheart;
    Image heartimage;

    private void Awake()
    {
        heartimage = GetComponent<Image>();
    }
    public void SetHeartStatus(HeartStatus status)
    {
        switch (status)
        {
            case HeartStatus.empty:
                heartimage.sprite = Emptyheart;
                break;
            case HeartStatus.full:
                heartimage.sprite = Fullheart;
                break;
        }
    }
}

public enum HeartStatus
{
    empty = 0,
    full = 1
}
