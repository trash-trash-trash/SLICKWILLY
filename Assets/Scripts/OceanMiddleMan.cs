using System;
using System.Collections;
using NUnit.Framework.Constraints;
using TMPro;
using UnityEngine;

public class OceanMiddleMan : MonoBehaviour
{
    public AnimalController animalController;
    public CameraManager camManager;
    public Cleaner cleaner;
    public GameObject comboObj;
    public DifficultyPicker difficultyPicker;
    public GameObject gameOverCanvasObj;
    public MenuManager menuManager;
    public MusicPlayer musicPlayer;
    public OceanTracker oceanTracker;
    public OceanGridGenerator oceanGenerator;
    public GameObject percentObj;
    public TankControls tankControls;
    public Timer timer;

    public TMP_Text aNewRecordText;
    public TMP_Text timeTaken;
    public TMP_Text comboText;

    public float acceptableEasyGamePercent = 100f;
    public float acceptableMediumGamePercent = 90f;
    public float acceptableHardGamePercent = 96f;

    public bool gameStartedFr = false;

    private void OnEnable()
    {
        menuManager.AnnounceGameStarted += StartGame;
        oceanTracker.AnnouncePercentClean += EndGame;
    }
    //     oceanGenerator.AnnounceOceanGenerated += SetRandomSkin;
    // }
    //
    // private void SetRandomSkin()
    // {
    //     if(!firstTime)
    //         return;
    //     
    //     squeegeeMaterialPicker.SetRandomSqueegeeMaterial();
    // }


    private void StartGame()
    {
        gameStartedFr = false;
        percentObj.SetActive(true);
        comboObj.SetActive(true);
        oceanTracker.percentClean = 0;
        oceanTracker.FlipGameStarted(true);
        musicPlayer.ResetOminousBaseline(oceanGenerator.waterPercent);

        StartCoroutine(HackWait());
    }

    IEnumerator HackWait()
    {
        yield return new WaitForSeconds(5f);
        gameStartedFr = true;
    }

    private void EndGame(float obj)
    {
        if (!gameStartedFr)
            return;
        
        float acceptableEndGamePercent = 95f;
        switch (difficultyPicker.currentDifficulty)
        {
            case Difficulty.Easy:
                acceptableEndGamePercent = acceptableEasyGamePercent;
                break;
            case Difficulty.Medium:
                acceptableEndGamePercent = acceptableMediumGamePercent;
                break;
            case Difficulty.Hard:
                acceptableEndGamePercent = acceptableHardGamePercent;
                break;
        }

        if (obj >= acceptableEndGamePercent)
        {
            percentObj.SetActive(false);

            comboObj.SetActive(false);
            gameOverCanvasObj.SetActive(true);

            comboText.text = "BIGGEST COMBO: " + cleaner.biggestCombo;

            timer.Stop();
            if (timer.aNewRecord)
                aNewRecordText.text = "A NEW RECORD!";
            else
                aNewRecordText.text = "";

            timeTaken.text = timer.bestTime.ToString();

            oceanTracker.FlipGameStarted(false);
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
        oceanTracker.FlipGameStarted(true);
    }

    void OnDisable()
    {
        menuManager.AnnounceGameStarted -= StartGame;
        oceanTracker.AnnouncePercentClean -= EndGame;
        // oceanGenerator.AnnounceOceanGenerated -= SetRandomSkin;
    }
}