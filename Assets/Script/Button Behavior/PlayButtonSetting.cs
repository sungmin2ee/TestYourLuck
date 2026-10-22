using UnityEngine;

public class PlayButtonSetting : MonoBehaviour
{
    
    public GameObject bettingPanel;
    public GameObject gamePanel;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (transform.parent != null && transform.parent.name == "GamePanel")
        {
            RectTransform button = GetComponent<RectTransform>();
            Canvas canvas = GetComponentInParent<Canvas>();
            RectTransform canvasRect = canvas.GetComponent<RectTransform>();
            float screenWidth = canvasRect.rect.width;
            float screenHeight = canvasRect.rect.height;
            float newButtonHeight = screenHeight / 10;
            button.sizeDelta = new Vector2(screenWidth / 2, newButtonHeight);
        }

    }
  
    
}
