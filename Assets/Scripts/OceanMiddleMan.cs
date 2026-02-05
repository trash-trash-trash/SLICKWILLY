using System;
using TMPro;
using UnityEngine;

public class OceanMiddleMan : MonoBehaviour
{
    public OceanTracker oceanTracker;
    public GameObject gameOverCanvasObj;

    public TankControls tankControls;
    
    public TMP_Text aNewRecordText;
    public TMP_Text timeTaken;

    public Timer timer;

    public MenuManager menuManager;
    
    private void Start()
    {
        oceanTracker.AnnouncePercentClean += EndGame;
    }

    public void Replay()
    {
        menuManager.cameraManager.SetUpGame();
    }

    private void EndGame(float obj)
    {
        tankControls.FlipControlOnOff(false);
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
