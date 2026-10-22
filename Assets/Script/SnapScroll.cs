using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class SnapScroll : MonoBehaviour
{
    public ScrollRect scrollRect;
    public RectTransform content;
    public int totalScreens;
    public float snapSpeed = 10f;
    public float minDragDistance = 70f; 

    private float screenWidth;
    private Vector2 startDragPosition;
    private Vector2 endDragPosition;
    private bool isDragging = false;
    private bool isSnapping = false;
    private Vector2 targetPosition;
    [SerializeField]
    private int currentScreen = 0;
    private float startContentX;

    void Start()
    {
     
        Canvas canvas = GetComponentInParent<Canvas>();
        screenWidth = canvas.GetComponent<RectTransform>().rect.width;

        if (content == null)
            content = scrollRect.content;

        if (totalScreens <= 0)
            totalScreens = content.childCount;
    }

    void Update()
    {
        HandleInput();

        if (isSnapping)
        {
            content.anchoredPosition = Vector2.Lerp(content.anchoredPosition, targetPosition, snapSpeed * Time.deltaTime);

            if (Vector2.Distance(content.anchoredPosition, targetPosition) < 1f)
            {
                content.anchoredPosition = targetPosition;
                isSnapping = false;
            }
        }
    }

    void HandleInput()
    {
        if (Touchscreen.current != null)
        {
            
            var touch = Touchscreen.current.primaryTouch;

            if (touch.press.wasPressedThisFrame)
            {
                isDragging = true;
                startDragPosition = touch.position.ReadValue();
                startContentX = content.anchoredPosition.x;
            }
            else if (touch.press.wasReleasedThisFrame)
            {
                isDragging = false;
                endDragPosition = touch.position.ReadValue();
                scrollRect.inertia = false;
                scrollRect.velocity = Vector2.zero;
                SnapBasedOnDirection();
            }
        }
        //For window debug
        else if (Mouse.current != null)
        {

            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                isDragging = true;
                startDragPosition = Mouse.current.position.ReadValue();
                startContentX = content.anchoredPosition.x;
            }
            else if (Mouse.current.leftButton.wasReleasedThisFrame)
            {
                isDragging = false;
                endDragPosition = Mouse.current.position.ReadValue();
                scrollRect.inertia = false;
                scrollRect.velocity = Vector2.zero;
                SnapBasedOnDirection();
            }
        }
    }

    //Snapping Screens
    void SnapBasedOnDirection()
    {
        float deltaX = endDragPosition.x - startDragPosition.x;
        Debug.Log(deltaX);

        if (Mathf.Abs(deltaX) > minDragDistance)
        {
            if (deltaX < 0)
            {
               
                currentScreen = Mathf.Clamp(currentScreen + 1, 0, totalScreens - 1);
            }
            else if (deltaX > 0)
            {
              
                currentScreen = Mathf.Clamp(currentScreen - 1, 0, totalScreens - 1);
            }
        }


        targetPosition = new Vector2(-currentScreen * screenWidth, 0f);
        isSnapping = true;
    }



}
