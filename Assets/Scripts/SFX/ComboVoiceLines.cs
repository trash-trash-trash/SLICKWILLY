using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ComboVoiceLines : MonoBehaviour
{
    public Cleaner cleaner;
    public AudioClip sustainable;
    public AudioSource source;
    public List<AudioClip> comboSounds = new List<AudioClip>();
    
    void OnEnable()
    {
        cleaner.AnnounceCurrentCombo += PlayRandomComboSound;
        cleaner.AnnounceBiggestCombo += Sustainable;
        
    }

    public void Sustainable(int i)
    {
        source.clip = sustainable;
        source.Play();
    }

    public void PlayRandomComboSound(int i)
    {
        if (i % 1000 == 0)
        {
            source.clip = comboSounds[Random.Range(0, comboSounds.Count)];
            source.Play();
        }

        if (i % 10000 == 0)
        {
            source.clip = sustainable;
            source.Play();
        }}
    
    void OnDisable()
    {
        cleaner.AnnounceCurrentCombo -= PlayRandomComboSound;
        cleaner.AnnounceBiggestCombo -= Sustainable;
        
    }

}