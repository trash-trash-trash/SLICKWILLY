using UnityEngine;

public class Bobbing : MonoBehaviour
{
    public float height = 0.25f;   
    public float speed = 1f;      

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.localPosition;
    }

    void Update()
    {
        float yOffset = Mathf.Sin(Time.time * speed) * height;
        transform.localPosition = startPos + Vector3.up * yOffset;
    }
}