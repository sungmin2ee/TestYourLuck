using UnityEngine;

public class camerafollow : MonoBehaviour
{
    public GameObject coin;
    public Vector3 offset;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        offset = transform.position - coin.transform.position;
        offset.y = Mathf.Max(offset.y, 1.0f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = coin.transform.position + offset;
    }
}
