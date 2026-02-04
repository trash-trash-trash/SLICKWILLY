using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public Canvas uiMainMenu;

    private void Start()
    {
        uiMainMenu.enabled = true;
    }
    
    public void StartBTN()
    {
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }
    
    public void TimeTrialsBTN()
    {
        SceneManager.LoadScene(2, LoadSceneMode.Single);
    }
    
    public void OptionsBTN()
    {
        
    }
    
    public void QuitBTN()
    {
        Application.Quit();
    }
}
