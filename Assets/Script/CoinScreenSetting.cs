using UnityEngine;
using UnityEngine.UI;

public class CoinScreenSetting : MonoBehaviour
{
    private RectTransform screen1;
    void Update()
    {
        screen1 = GameObject.Find("Screen1").GetComponent<RectTransform>();
        float screenWidth = screen1.rect.width;
        float screenHeight = screen1.rect.height;

        RectTransform rawImage = GetComponent<RectTransform>();
      
        rawImage.sizeDelta = new Vector2(screenWidth, screenHeight);
        
        //Vector3 currentScale = rawImage.localScale;
        //rawImage.localScale = new Vector3(1.5f, currentScale.y, currentScale.z);
      
    }

}
