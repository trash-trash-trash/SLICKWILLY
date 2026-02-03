using System;
using UnityEngine;

public class DolphinView : MonoBehaviour
{
    public MeshRenderer mesh;
    
    public Material oilMat;

    public Material cleanMat;
    
    private void OnEnable()
    {
        SetMaterials(cleanMat);
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
