using UnityEngine;

public class AnimalNoise : MonoBehaviour
{
    public OilComponent oilComponent;
    
    public AudioSource audioSource;

    void OnEnable()
    {
        oilComponent.AnnounceCleanOrOily += MakeNoise;
    }

    private void MakeNoise(OilComponent arg1, bool arg2)
    {
        if (arg2)
        {
            audioSource.Play();
        }
    }

    void OnDisable()
    {
        oilComponent.AnnounceCleanOrOily -= MakeNoise;
    }
}
