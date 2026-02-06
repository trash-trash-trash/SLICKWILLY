using System.Collections.Generic;
using UnityEngine;

public class TugBoatView : MonoBehaviour
{
    public TankControls model;

    public ParticleSystem splashParticles;

    private bool lastIsOily;
    
    private void OnEnable()
    {
        model.MovingEvent += MovingEvent;
    }

    private void MovingEvent(bool obj)
    {
        if (obj)
        {
            splashParticles.Play();
        }
        else
        {
            splashParticles.Stop();
        }
    }
    void FixedUpdate()
    {
        if (model.currentSpeedMultiplier == model.oilSpeedMultiplier)
            return;
        
        lastIsOily = model.isOily;
        
        var colour = splashParticles.colorOverLifetime;
        
        if (model.isOily)
        {
            colour.color = new ParticleSystem.MinMaxGradient(Color.black, Color.clear);
        }
        else
        {
            colour.color = new ParticleSystem.MinMaxGradient(Color.white, Color.clear);
        }
    }
}
