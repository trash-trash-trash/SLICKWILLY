using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MotherShipView : MonoBehaviour
{
    public MotherShipModel model;
    public AudioSource shipAudio;

    public float minBargeSoundLength;
    public float maxBargeSoundLength;

    private float timer;

    public List<ParticleSystem> splashParticles;

    private void OnEnable()
    {
        model.movingEvent += MovingEvent;
    }

    private void MovingEvent(bool obj)
    {
        foreach (ParticleSystem particle in splashParticles)
        {
            if (obj)
            {
                particle.Play();
            }
            else
            {
                particle.Stop();
            }
        }
    }

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
