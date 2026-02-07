using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class MusicClip
{
    public AudioSource source;
    public bool heavy;
}

[System.Serializable]
public class Song
{
    public List<MusicClip> clips;
}

public class MusicPlayer : MonoBehaviour
{
    public List<Song> songs = new List<Song>();
    public float blendSpeed = 1f;
    public float waitTime = 4f;

    public OceanTracker oceanTracker;

    public float  ominousCurrent = 1f;
    public float ominousTarget = 1f;
    public float startCleanPercent = 100f;

    int songIndex = 0;

    void OnEnable()
    {
        oceanTracker.AnnouncePercentClean += SetOminous;
        StartCoroutine(SongLoop());
    }

    void Update()
    {
        ominousCurrent = Mathf.MoveTowards(
            ominousCurrent,
            ominousTarget,
            blendSpeed * Time.deltaTime
        );

        // Only touch currently playing sources
        Song song = songs[songIndex];
        foreach (var mc in song.clips)
        {
            if (!mc.source.isPlaying)
                continue;

            mc.source.volume = mc.heavy ? ominousCurrent : 1f;
        }
    }

    IEnumerator SongLoop()
    {
        while (true)
        {
            Song song = songs[songIndex];
            float longest = 0f;

            foreach (var mc in song.clips)
            {
                if (mc.heavy && ominousCurrent <= 0f)
                    continue;

                mc.source.volume = mc.heavy ? ominousCurrent : 1f;
                mc.source.Play();

                longest = Mathf.Max(longest, mc.source.clip.length);
            }

            yield return new WaitForSeconds(longest + waitTime);

            foreach (var mc in song.clips)
            {
                mc.source.Stop();
            }

            songIndex = (songIndex + 1) % songs.Count;
        }
    }

    public void ResetOminousBaseline(float percentClean)
    {
        startCleanPercent = percentClean;
        ominousCurrent = 1f;
        ominousTarget = 1f;
    }

    void SetOminous(float percentClean)
    {
        float t = Mathf.InverseLerp(startCleanPercent, 100f, percentClean);
        ominousTarget = 1f - t;
    }

    void OnDisable()
    {
        oceanTracker.AnnouncePercentClean -= SetOminous;
    }
}