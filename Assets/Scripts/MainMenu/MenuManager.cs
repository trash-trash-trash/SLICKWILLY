using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject uiMainMenu;
    public GameObject setupGameMenu;

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

    public void QuitBTN()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
    }
}