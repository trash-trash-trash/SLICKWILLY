using System;
using UnityEngine;

public class OilComponent : MonoBehaviour
{
    [SerializeField] private bool isClean = true;

    public bool IsClean => isClean;
    public bool IsOily => !isClean;

    public event Action<OilComponent, bool> AnnounceCleanOrOily;

    public void Clean()
    {
        if (isClean) return;

        isClean = true;
        AnnounceCleanOrOily?.Invoke(this, isClean);
    }

    public void Dirty()
    {
        if (!isClean) return;

        isClean = false;
        UpdateVisuals();
        AnnounceCleanOrOily?.Invoke(this, isClean);
    }

    private void UpdateVisuals()
    {
        //swamp models, material
    }
}