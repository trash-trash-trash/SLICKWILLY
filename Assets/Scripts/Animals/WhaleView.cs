using UnityEngine;

public class WhaleView : MonoBehaviour
{
    public SkinnedMeshRenderer mesh;
    
    public Material oilMat;

    public Material cleanMat;

    public OilComponent oilComponent;
    
    private void OnEnable()
    {
        oilComponent.AnnounceCleanOrOily += CleanDirty;
        DirtyWhale();
    }

    private void CleanDirty(OilComponent arg1, bool arg2)
    {
        if(arg2)
            CleanWhale();
        else
            DirtyWhale();
    }

    public void DirtyWhale()
    {
        SetMaterials(oilMat);
    }
    
    public void CleanWhale()
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
