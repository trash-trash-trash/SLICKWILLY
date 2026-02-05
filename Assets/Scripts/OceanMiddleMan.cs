using System;
using TMPro;
using UnityEngine;

public class OceanMiddleMan : MonoBehaviour
{
    public OceanTracker oceanTracker;
    public GameObject gameOverCanvasObj;
    public TMP_Text aNewRecordText;
    public TMP_Text timeTaken;

    public Timer timer;

    private void Start()
    {
        oceanTracker.AnnouncePercentClean += EndGame;
    }

    private void EndGame(float obj)
    {
        if (obj == 100)
        {
            gameOverCanvasObj.SetActive(true);
            
            timer.Stop();
            if (timer.aNewRecord)
                aNewRecordText.text = "A NEW RECORD!";
            else 
                aNewRecordText.text = "";
            
            timeTaken.text = timer.bestTime.ToString();
        }
    }
}
