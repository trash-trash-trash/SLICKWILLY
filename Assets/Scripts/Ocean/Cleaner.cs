using UnityEngine;

public class Cleaner : MonoBehaviour
{
    [Header("Growth Settings")] 
    
    public Vector3 initialScale;
    public float growthMultiplier = 1f;

    public OceanTracker tracker;

    public Transform targetTransform;

    private void OnEnable()
    {
        initialScale = targetTransform.localScale;
        tracker = FindObjectOfType<OceanTracker>();
        tracker.AnnouncePercentClean += OnPercentCleanChanged;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<OilComponent>() != null)
        {
            OilComponent oil = other.GetComponent<OilComponent>();
            if (oil.IsOily)
            {
                oil.Clean();
            }
        }
    }

    private void OnPercentCleanChanged(float percentClean)
    {
        
        float scaleFactor = percentClean * growthMultiplier / 100f;
        targetTransform.localScale = initialScale * scaleFactor;
    }

    private void OnDisable()
    {
        tracker.AnnouncePercentClean -= OnPercentCleanChanged;
    }
}