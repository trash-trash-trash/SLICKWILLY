using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    //normal
    public AudioSource sourceA; 
    //oily
    public AudioSource sourceB;

    [Range(0f, 1f)]
    public float ominousTarget = 0f;

    public float blendSpeed = 2f;

    private float ominousCurrent = 0f;

    public OceanTracker oceanTracker;

    void Start()
    {
        oceanTracker.AnnouncePercentClean += SetOminous;
        StartSong();
    }

    //make it play on game start
    void StartSong()
    {
        sourceA.loop = true;
        sourceB.loop = true;

        // start synced
        sourceA.volume = 1f;
        sourceB.volume = 0f;

        sourceA.Play();
        sourceB.Play();
    }

    void Update()
    {
        ominousCurrent = Mathf.MoveTowards(
            ominousCurrent,
            ominousTarget,
            blendSpeed * Time.deltaTime
        );

        sourceA.volume = Mathf.Cos(ominousCurrent * Mathf.PI * 0.5f);
        sourceB.volume = Mathf.Sin(ominousCurrent * Mathf.PI * 0.5f);
    }

    public void SetOminous(float percentClean)
    {
        float clean01 = Mathf.Clamp01(percentClean / 100f);
        ominousTarget = 1f - clean01;
    }

    void OnDisable()
    {
        oceanTracker.AnnouncePercentClean -= SetOminous;
    }
}