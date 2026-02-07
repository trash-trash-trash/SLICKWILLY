using System;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class VoiceLinePlayer : MonoBehaviour
{
    public AnimalController animalController;
    //how to listen to individual animal identifiers
    public List<AudioClip> dolphinVoicelines =  new List<AudioClip>();
    public List<AudioClip> whaleVoicelines =  new List<AudioClip>();
    
    public AudioSource source;

    void OnEnable()
    {
        animalController.AnnounceAnimalsSpawned += SubToAnimalsList;
    }
    
    //TODO: FIX
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
    
    public void UnSubToAnimalsList()
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
            
            arg1.AnnounceCleanOrOily -= AttemptDolphin;
        }
    }
   private void AttemptWhale(OilComponent arg1, bool arg2)
    {
        if (arg2)
        {
            PlayRandomAnimalVoiceLine(1);
            arg1.AnnounceCleanOrOily -= AttemptWhale;
        }
    }

    private void PlayRandomAnimalVoiceLine(int obj)
    {
        
        List<AudioClip> chosenList = null;

        if (obj == 0)
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
            
            chosenList = dolphinVoicelines;
        }
        
        else if (obj == 1)
            chosenList = whaleVoicelines;

        AudioClip clipToPlay = chosenList[UnityEngine.Random.Range(0, chosenList.Count)];
        
        source.PlayOneShot(clipToPlay);
    }

    void OnDisable()
    {
        animalController.AnnounceAnimalsSpawned -= SubToAnimalsList;
    }
}
