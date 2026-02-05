using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject uiMainMenu;
    public GameObject setupGameMenu;
    public GameObject percentCleanObj;
    
    public TankControls tankControls;
    public CameraManager cameraManager;

    public GameObject sampleScene;

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

    public void StartGame()
    {
        cameraManager.RunGame();
        percentCleanObj.SetActive(true);
        setupGameMenu.SetActive(false);
        tankControls.FlipControlOnOff(true);
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