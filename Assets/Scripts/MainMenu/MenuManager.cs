using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject uiMainMenu;
    public GameObject setupGameMenu;
    public GameObject percentCleanObj;
    
    public TankControls tankControls;

    private void Start()
    {
        uiMainMenu.SetActive(true);
        setupGameMenu.SetActive(false);
    }

    public void SetupGame()
    {
        uiMainMenu.SetActive(false);
        setupGameMenu.SetActive(true);
    }

    public void StartGame()
    {
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