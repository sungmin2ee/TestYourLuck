using UnityEngine;
using UnityEngine.UI;
public class ScreenSetting : MonoBehaviour
{
    public int totalScreens;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        totalScreens = transform.childCount;
        Canvas canvas = GetComponentInParent<Canvas>();
        RectTransform canvasRect = canvas.GetComponent<RectTransform>();
        float screenWidth = canvasRect.rect.width;
        float screenHeight = canvasRect.rect.height;

        // BigPanel 크기 조정
        RectTransform bigPanel = GetComponent<RectTransform>();
        bigPanel.sizeDelta = new Vector2(screenWidth * totalScreens, screenHeight);

        // Viewport 크기 조정
        RectTransform viewportRect = transform.parent.GetComponent<RectTransform>();
        viewportRect.sizeDelta = new Vector2(screenWidth, screenHeight);

        // 각 Screen 크기 조정
        for (int i = 0; i < totalScreens; i++)
        {
            RectTransform screen = transform.GetChild(i).GetComponent<RectTransform>();
            screen.sizeDelta = new Vector2(screenWidth, screenHeight);
            screen.anchoredPosition = new Vector2(i * screenWidth, 0); // 옆으로 배치
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
