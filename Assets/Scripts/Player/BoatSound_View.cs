using UnityEngine;

public class BoatSound_View : MonoBehaviour
{
	public AudioSource audioSource;
	public TankControls tankControls;
	
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
	    audioSource.volume = Mathf.Lerp(audioSource.volume, tankControls.rb.linearVelocity.magnitude/25f, 10.1f * Time.deltaTime);
    }
}
