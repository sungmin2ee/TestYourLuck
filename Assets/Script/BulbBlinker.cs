using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BulbBlinker : MonoBehaviour
{
    public List<Image> bulbs;
    public float speed = 2f;
    public Color originalColor = Color.yellow;
    public Color dimColor = Color.gray;


    // Update is called once per frame
    void Update()
    {
        float t = (Mathf.Sin(Time.time * speed) + 1f) / 2f;
        foreach (var bulb in bulbs)
        {
            bulb.color = Color.Lerp(originalColor, dimColor, t);
        }
    }
}
