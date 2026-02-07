using TMPro;
using UnityEngine;

public class OceanTrackerView : MonoBehaviour
{
    public OceanTracker oceanTracker;
    
    public TMP_Text percentText;

    void OnEnable()
    {
        oceanTracker.AnnouncePercentClean += SetPercentText;
    }

    private void SetPercentText(float obj)
    {
        percentText.text = obj.ToString("F2") + "%";
    }

    void OnDisable()
    {
        oceanTracker.AnnouncePercentClean -= SetPercentText;
    }
}
