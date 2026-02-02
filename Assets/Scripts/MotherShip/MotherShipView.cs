using UnityEngine;

public class MotherShipView : MonoBehaviour
{
    public AudioSource shipAudio;

    public float minBargeSoundLength;
    public float maxBargeSoundLength;

    private float timer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ResetTimer();
    }

    void ResetTimer()
    {
        timer = Random.Range(minBargeSoundLength, maxBargeSoundLength);
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            shipAudio.Play();
            ResetTimer();
        }
    }
}
