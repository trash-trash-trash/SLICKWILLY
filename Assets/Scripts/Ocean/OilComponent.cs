using System;
using UnityEngine;

public class OilComponent : MonoBehaviour
{
    [SerializeField] private bool isClean = true;

    public bool IsClean => isClean;
    public bool IsOily => !isClean;

    public event Action<OilComponent, bool> AnnounceCleanOrOily;

    public bool canBeCleaned = true;

    public void Clean()
    {
        if (isClean)
            return;

        if (!canBeCleaned)
            return;
        
        isClean = true;
        AnnounceCleanOrOily?.Invoke(this, isClean);
    }

    public void FlipCanBeCleaned(bool input)
    {
        canBeCleaned = input;
    }

    public void Dirty()
    {
        if (!isClean) 
            return;

        if (!canBeCleaned)
            return;
        
        isClean = false;
        UpdateVisuals();
        AnnounceCleanOrOily?.Invoke(this, isClean);
    }

    private void UpdateVisuals()
    {
        //swamp models, material
    }
}