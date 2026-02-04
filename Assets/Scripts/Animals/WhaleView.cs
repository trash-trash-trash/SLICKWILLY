using UnityEngine;

public class WhaleView : MonoBehaviour
{
    public SkinnedMeshRenderer mesh;
    
    public Material oilMat;

    public Material cleanMat;
    
    private void OnEnable()
    {
        SetMaterials(cleanMat);
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
