using System;
using System.Collections.Generic;
using UnityEngine;

public class FishParticles : MonoBehaviour
{
    public List<ParticleSystem> fishParticleSystems;

    public void OnEnable()
    {
        // OtherScript.eventName += PlayFishParticles;
    }

    public void PlayFishParticles()
    {
        foreach (ParticleSystem system in fishParticleSystems)
        {
            system.Play();
        }
    }
    
    public void OnDisable()
    {
        // OtherScript.eventName -= PlayFishParticles;
    }
}
