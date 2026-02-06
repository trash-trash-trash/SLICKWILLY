using System;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class VoiceLinePlayer : MonoBehaviour
{
    public AnimalController animalController;
    
    public List<AudioClip> comboSounds =  new List<AudioClip>();
    //how to listen to individual animal identifiers
    public List<AudioClip> dolphinVoicelines =  new List<AudioClip>();
    public List<AudioClip> whaleVoicelines =  new List<AudioClip>();
    
    public AudioSource source;

    void OnEnable()
    {
        animalController.AnnounceAnimalsSpawned += SubToAnimalsList;
    }

    private void SubToAnimalsList()
    {
        foreach (GameObject dolphinObj in animalController.dolphinsSpawned)
        {
            OilComponent oilComponent = dolphinObj.GetComponent<OilComponent>();
            oilComponent.AnnounceCleanOrOily += AttemptDolphin;
        }
        
        foreach (GameObject whaleObj in animalController.whalesSpawned)
        {
            OilComponent oilComponent = whaleObj.GetComponent<OilComponent>();
            oilComponent.AnnounceCleanOrOily += AttemptWhale;
        }
    }
    
    private void UnSubToAnimalsList()
    {
        foreach (GameObject dolphinObj in animalController.dolphinsSpawned)
        {
            OilComponent oilComponent = dolphinObj.GetComponent<OilComponent>();
            oilComponent.AnnounceCleanOrOily -= AttemptDolphin;
        }
        
        foreach (GameObject whaleObj in animalController.whalesSpawned)
        {
            OilComponent oilComponent = whaleObj.GetComponent<OilComponent>();
            oilComponent.AnnounceCleanOrOily -= AttemptWhale;
        }
        
    }

    //TODO: refactor (copy pasting)
    
    private void AttemptDolphin(OilComponent arg1, bool arg2)
    {
        if (arg2)
        {
            PlayRandomAnimalVoiceLine(0);
        }
    }
   private void AttemptWhale(OilComponent arg1, bool arg2)
    {
        if (arg2)
        {
            PlayRandomAnimalVoiceLine(1);
        }
    }

    private void PlayRandomAnimalVoiceLine(int obj)
    {
        Debug.Log("Attempting Voice Line");
        // 50% chance to play at all
        int random = UnityEngine.Random.Range(0,1);
        if (random > 0.5f)
        {
            Debug.Log("Failed Voice Line");
            return;
        }
        
        Debug.Log("Completed Voice Line");

        List<AudioClip> chosenList = null;

        if (obj == 0)
            chosenList = dolphinVoicelines;
        else if (obj == 1)
            chosenList = whaleVoicelines;

        if (chosenList == null || chosenList.Count == 0) return;

        AudioClip clipToPlay = comboSounds[UnityEngine.Random.Range(0, chosenList.Count)];
        
        source.PlayOneShot(clipToPlay);
    }
    
    public void PlayRandomComboSound()
    {
        source.clip = comboSounds[UnityEngine.Random.Range(0, comboSounds.Count)];
        source.Play();
    }

    void OnDisable()
    {
        animalController.AnnounceAnimalsSpawned -= SubToAnimalsList;
    }
}
