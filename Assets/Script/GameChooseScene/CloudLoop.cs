using UnityEngine;

public class UICloudLoop : MonoBehaviour
{
    public float speed = 100f;
    private RectTransform rt;
    private float screenWidth;
    public float floatStrength = 0.05f;
    void Start()
    {
        rt = GetComponent<RectTransform>();

        // 캔버스 너비 기준 계산
        screenWidth = rt.root.GetComponent<Canvas>().GetComponent<RectTransform>().rect.width;
    }

    void Update()
    {
        // 왼쪽으로 이동
        rt.anchoredPosition += Vector2.left * speed * Time.deltaTime;
        float yOffset = Mathf.Sin(Time.time * 2f) * floatStrength;
        rt.anchoredPosition = rt.anchoredPosition  +new Vector2(0, yOffset);
        // 구름이 화면 왼쪽 끝보다 더 나가면 → 오른쪽 끝으로 이동
        if (rt.anchoredPosition.x < -screenWidth / 2 - rt.rect.width)
        {
            rt.anchoredPosition = new Vector2(screenWidth / 2 + rt.rect.width, rt.anchoredPosition.y);
        }
    }
}
