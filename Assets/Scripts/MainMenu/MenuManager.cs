using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject uiMainMenu;
    public GameObject howToPlayObject;
    public GameObject setupGameMenu;
    public GameObject percentCleanObj;
    
    public TankControls tankControls;
    public CameraManager cameraManager;

    public GameObject sampleScene;
    public event Action AnnounceGameStarted;

    //double check with replaying game
    private void Start()
    {
        sampleScene.SetActive(true);
        uiMainMenu.SetActive(true);
        setupGameMenu.SetActive(false);
        cameraManager.MenuCameraFocus();
    }

    public void SetupGame()
    {
        cameraManager.SetUpGame();
        sampleScene.SetActive(false);
        uiMainMenu.SetActive(false);
        setupGameMenu.SetActive(true);
    }

    public void HowToPlay()
    {
        uiMainMenu.SetActive(false);
        howToPlayObject.SetActive(true);
    }

    public void Back()
    {
        howToPlayObject.SetActive(false);
        uiMainMenu.SetActive(true);
    }

    public void StartGame()
    {
        cameraManager.RunGame();
        percentCleanObj.SetActive(true);
        setupGameMenu.SetActive(false);
        tankControls.FlipControlOnOff(true);
        AnnounceGameStarted?.Invoke();
    }

    public void QuitBTN()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
    }
}