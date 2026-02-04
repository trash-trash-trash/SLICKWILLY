using System;
using UnityEngine;

public class DolphinView : MonoBehaviour
{
    public MeshRenderer mesh;
    
    public Material oilMat;

    public Material cleanMat;

    public OilComponent oilComponent;
    
    private void OnEnable()
    {
        DirtyDolphin();
        oilComponent.AnnounceCleanOrOily += SetDirtyClean;
    }

    private void SetDirtyClean(OilComponent arg1, bool arg2)
    {
        if (arg2)
            CleanDolphin();
        else
            DirtyDolphin();
    }

    public void DirtyDolphin()
    {
        SetMaterials(oilMat);
    }
    
    public void CleanDolphin()
    {
        SetMaterials(cleanMat);
    }

    void SetMaterials(Material mat)
    {
        Material[] mats = mesh.materials;
        mats[0] = mat;
        mesh.materials = mats;
    }
}
