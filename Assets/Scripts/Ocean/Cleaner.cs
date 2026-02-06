using System;
using UnityEngine;

public class Cleaner : MonoBehaviour
{
	public ParticleSystem splashParticles;

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

	void OnEnable()
	{
		Physics.IgnoreLayerCollision(8, 10);
	}
	
	private void OnTriggerEnter(Collider other)
    {
	    var colour = splashParticles.colorOverLifetime;

	    if (other.GetComponent<OilComponent>()!=null)
        {
            OilComponent oil = other.GetComponent<OilComponent>();
            oil.Clean();
            colour.color = new ParticleSystem.MinMaxGradient(Color.black, Color.clear);
        }
        else
        {
	        colour.color = new ParticleSystem.MinMaxGradient(Color.white, Color.clear);
        }
    }
    //
    // private void OnTriggerStay(Collider other)
    // {
    //     // Optional: if you want continuous cleaning
    //     // (ex: cleaning progress per second)
    // }
}