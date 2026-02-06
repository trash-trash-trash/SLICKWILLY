using System;
using UnityEngine;

public class Cleaner : MonoBehaviour
{
	// [SerializeField]
	// private float radius = 5f;
	//
	// private void FixedUpdate()
	// {
	// 	Collider[] colliders = new Collider[50];
	// 	Physics.OverlapSphereNonAlloc(transform.position, radius, colliders);
	//
	// 	if (colliders.Length > 0)
	// 	{
	// 		for (int i = 0; i < colliders.Length; i++)
	// 		{
	// 			colliders[i].GetComponent<OilComponent>()?.Clean();
	// 		}
	// 	}
	// }

	private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<OilComponent>()!=null)
        {
            OilComponent oil = other.GetComponent<OilComponent>();
            oil.Clean();
        }
    }
    //
    // private void OnTriggerStay(Collider other)
    // {
    //     // Optional: if you want continuous cleaning
    //     // (ex: cleaning progress per second)
    // }
}