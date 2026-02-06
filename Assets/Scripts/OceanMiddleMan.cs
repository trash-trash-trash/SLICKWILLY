using System;
using TMPro;
using UnityEngine;

public class OceanMiddleMan : MonoBehaviour
{
    public AnimalController animalController;
    public CameraManager camManager;
    public GameObject gameOverCanvasObj;
    public MenuManager menuManager;
    public OceanTracker oceanTracker;
    public OceanGridGenerator oceanGenerator;
    public TankControls tankControls;
    public Timer timer;

    public TMP_Text aNewRecordText;
    public TMP_Text timeTaken;

    
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

    public void Replay()
    {
        animalController.ResetAnimals();
        gameOverCanvasObj.SetActive(false);
        camManager.SetUpGame();
        tankControls.FlipControlOnOff(false);
        //lmao
        tankControls.gameObject.transform.position = new Vector3(110f, 2.03f, 100f);
        menuManager.setupGameMenu.SetActive(true);
        oceanGenerator.SpawnGrid();
    }

    void OnDisable()
    {
        oceanTracker.AnnouncePercentClean -= EndGame;
    }
}
