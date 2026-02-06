using System;
using UnityEngine;

public class OceanTile : MonoBehaviour
{
    public OceanType oceanType = OceanType.Water;
    
    public Renderer renderer;

    public OilComponent oilComponent;

    void OnEnable()
    {
        oilComponent.AnnounceCleanOrOily += CleanDirty;
        
        CleanDirty(oilComponent, true);
    }
    
    public void CleanDirty(OilComponent component, bool b)
    {
        if(b)
        {
            oceanType = OceanType.Water;
            renderer.enabled = false;
            gameObject.name = "Water";
        }
        else
        {
            renderer.enabled = true;
            oceanType = OceanType.Oil;
            gameObject.name = "Oil";
        }
    }

    void OnDisable()
    {
        oilComponent.AnnounceCleanOrOily -= CleanDirty;
    }
}