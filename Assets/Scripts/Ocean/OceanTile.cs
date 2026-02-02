using System;
using UnityEngine;

public class OceanTile : MonoBehaviour, IOceanTile
{
    public OceanType OceanType = OceanType.Water;

    private bool cleanWater = false;

    //probably needs an ID?
    public event Action<OceanTile> AnnounceCleaned;
    
    public bool CleanWater
    {
        get => cleanWater;
        private set => cleanWater = value;
    }

    public void Clean()
    {
        CleanWater = true;
        AnnounceCleaned?.Invoke(this);
    }
    
    
}
