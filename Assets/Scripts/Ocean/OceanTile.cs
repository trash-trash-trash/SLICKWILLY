using System;
using UnityEngine;

public class OceanTile : MonoBehaviour
{
    public OceanType oceanType = OceanType.Water;
   
    public Material oilMaterial;
    public Material waterMaterial;

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
            renderer.sharedMaterial = waterMaterial;
            oceanType = OceanType.Water;
        }
        else
        {
            renderer.sharedMaterial = oilMaterial;
            oceanType = OceanType.Oil;
        }
    }
}