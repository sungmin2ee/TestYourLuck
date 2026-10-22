using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class OutlineManager : MonoBehaviour
{
    private static List<OutlineToggle> allOutlines = new List<OutlineToggle>();
    private static string selected;

    public LayerMask clickableLayer; 

    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame || Touchscreen.current.primaryTouch.press.wasPressedThisFrame)
        {
            //Vector2 mousePos = Mouse.current.position.ReadValue();
            //Ray ray = Camera.main.ScreenPointToRay(mousePos);
            Vector2 touchPos = Touchscreen.current.primaryTouch.position.ReadValue();
            Ray ray = Camera.main.ScreenPointToRay(touchPos);

            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, clickableLayer))
            {
                OutlineToggle outline = hit.collider.GetComponent<OutlineToggle>();
                if (outline != null)
                {
                    Debug.Log("Clicked: " + hit.collider.name);
                    selected = hit.collider.name;

                    foreach (var other in allOutlines)
                    {
                        if (other != outline && other.IsOutlined())
                            other.ToggleOutlineOff();
                    }

                    outline.ToggleOutline();
                }
            }
        }
    }


    public static void Register(OutlineToggle toggle)
    {
        if (!allOutlines.Contains(toggle))
            allOutlines.Add(toggle);
    }

    public static void Unregister(OutlineToggle toggle)
    {
        if (allOutlines.Contains(toggle))
            allOutlines.Remove(toggle);
    }

    public static string GetSelected()
    {
        return selected;
    }
}
