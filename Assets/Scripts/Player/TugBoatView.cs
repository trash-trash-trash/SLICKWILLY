using System.Collections.Generic;
using UnityEngine;

public class TugBoatView : MonoBehaviour
{
    public TankControls model;

    public ParticleSystem splashParticles;

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
        if (model.isOily)
        {
            var splashParticlesColorOverLifetime = splashParticles.colorOverLifetime;
            splashParticlesColorOverLifetime.color = new ParticleSystem.MinMaxGradient(Color.black, Color.clear);
        }
        else
        {
            var splashParticlesColorOverLifetime = splashParticles.colorOverLifetime;
            splashParticlesColorOverLifetime.color = new ParticleSystem.MinMaxGradient(Color.white, Color.clear);
        }
    }
}
