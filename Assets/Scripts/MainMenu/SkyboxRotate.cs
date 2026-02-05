using UnityEngine;

public class SkyboxRotation : MonoBehaviour
{
    public float rotationSpeed = 0.7f; 

    void Update()
    {
        RenderSettings.skybox.SetFloat(
            "_Rotation",
            RenderSettings.skybox.GetFloat("_Rotation") + rotationSpeed * Time.deltaTime
        );
    }
}