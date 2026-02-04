using UnityEngine;

public class Cleaner : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<OilComponent>()!=null)
        {
            OilComponent oil = other.GetComponent<OilComponent>();
            oil.Clean();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        // Optional: if you want continuous cleaning
        // (ex: cleaning progress per second)
    }
}