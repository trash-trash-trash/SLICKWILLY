using System;
using Unity.Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public CinemachineCamera playerCam;

    public CinemachineCamera overallCam;

    public CinemachineCamera menuCam;
    
    public TankControls tankControls;

    public bool runningGame;

    private void Start()
    {
        runningGame = false;
    }

    private void OnEnable()
    {
        tankControls.CameraEvent += CameraEvent;
    }

    public void SetUpGame()
    {
        menuCam.Priority.Value = 0;
        playerCam.Priority.Value = 0;
        overallCam.Priority.Value = 1;
    }

    public void RunGame()
    {
        menuCam.Priority.Value = 0;
        playerCam.Priority.Value = 1;
        overallCam.Priority.Value = 0;
        runningGame = true;
    }

    public void MenuCameraFocus()
    {
        menuCam.Priority.Value = 1;
        playerCam.Priority.Value = 0;
        overallCam.Priority.Value = 0;
        runningGame = false;
    }

    private void CameraEvent(bool obj)
    {
        if (runningGame)
        {
            if (obj)
            {
                playerCam.Priority.Value = 0;
                overallCam.Priority.Value = 1;
            }
            else
            {
                playerCam.Priority.Value = 1;
                overallCam.Priority.Value = 0;
            }
        }
    }

    private void OnDisable()
    {
        tankControls.CameraEvent -= CameraEvent;
    }
}
