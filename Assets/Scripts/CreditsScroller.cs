using UnityEngine;
using TMPro;

public class CreditsScroller : MonoBehaviour
{
    public TMP_Text creditsText;
    public float scrollSpeed = 50f;

    private RectTransform textRect;
    private float textWidth=2198;
    private float startX;

    void OnEnable()
    {
        textRect = creditsText.GetComponent<RectTransform>();
        startX = Screen.width;
        Vector3 pos = textRect.anchoredPosition;
        pos.x = startX;
        textRect.anchoredPosition = pos;
    }

    void Update()
    {
        Vector3 pos = textRect.anchoredPosition;
        pos.x -= scrollSpeed * Time.deltaTime;

        if (pos.x < -textWidth)
        {
            pos.x = startX;
        }

        textRect.anchoredPosition = pos;
    }
}