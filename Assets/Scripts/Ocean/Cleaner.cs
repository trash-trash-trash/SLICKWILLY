using System;
using System.Collections.Generic;
using UnityEngine;

public class Cleaner : MonoBehaviour
{
	public int tilesCleanedTotal = 0;
	public int currentCombo = 0;
	public int biggestCombo = 0;

	public bool comboGainedInTheLastFiveFrames = false;
	public float comboTimer = 5f;
	private float comboCountdown = 0f;
	private int framesSinceLastCombo = 0;

	public List<OilComponent> oilCleaned = new List<OilComponent>();
	public event Action<int> AnnounceCurrentCombo;
	public event Action<int> AnnounceBiggestCombo;

	public bool active = false;


	void OnEnable()
	{
		Physics.IgnoreLayerCollision(8, 10);
	}
	
	private void Update()
	{
		HandleComboTimer();
	}
	
	private void HandleComboTimer()
	{
		if (currentCombo <= 0)
			return;

		//count frames since last combo gain
		framesSinceLastCombo++;

		//if combo was gained winth the last 5 frames reset countdown
		if (comboGainedInTheLastFiveFrames)
		{
			comboCountdown = comboTimer;
			comboGainedInTheLastFiveFrames = false;
			framesSinceLastCombo = 0;
		}

		// Decay timer
		comboCountdown -= Time.deltaTime;

		//if timer runs out, reset combo
		if (comboCountdown <= 0f)
		{
			CheckBiggestCombo();
			currentCombo = 0;
			comboCountdown = 0f;
		}
	}

	public void CheckBiggestCombo()
	{
		if (currentCombo > biggestCombo)
		{
			biggestCombo = currentCombo;
			AnnounceBiggestCombo?.Invoke(biggestCombo);
		}
	}

	
	void IncreaseCombo()
	{
		currentCombo++;
		comboGainedInTheLastFiveFrames = true;
		tilesCleanedTotal++;
		AnnounceCurrentCombo?.Invoke(currentCombo);
	}
	
	//clean
	
	private void OnTriggerEnter(Collider other)
	{
		if (!active)
			return;
		
	    if (other.GetComponent<OilComponent>()!=null)
        {
            OilComponent oil = other.GetComponent<OilComponent>();
            if(oil.canBeCleaned)
            {
	            oil.Clean();
	            
	            if(!oilCleaned.Contains(oil))
	            {
		            oilCleaned.Add(oil);
		            IncreaseCombo();
	            }
            }
        }
    }
	
}