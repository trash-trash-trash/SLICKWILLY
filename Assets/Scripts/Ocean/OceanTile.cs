using System;
using UnityEngine;

public class OceanTile : MonoBehaviour
{
    public OceanType oceanType = OceanType.Water;
    
    private bool cleanWater = false;
    public Renderer renderer;

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
    
    
    public void CleanDirty(bool clean)
    {
        if(clean)
        {
            renderer.enabled = false;
            oceanType = OceanType.Water;
        }
        else
        {
            renderer.enabled = true;
            oceanType = OceanType.Oil;
        }
    }
}