using System;
using Unity.Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public CinemachineCamera playerCam;

    public CinemachineCamera overallCam;
    
    public TankControls tankControls;
    private void OnEnable()
    {
        tankControls.CameraEvent += CameraEvent;
    }

    private void Start()
    {
        playerCam.Priority.Value = 1;
        overallCam.Priority.Value = 0;
    }

    private void CameraEvent(bool obj)
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

    private void OnDisable()
    {
        tankControls.CameraEvent -= CameraEvent;
    }
}
