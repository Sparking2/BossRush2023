using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    public CinemachineBrain _cameraBrain;
    private bool inMenu = true;
    public CinemachineVirtualCamera _menuCamera;
    public CinemachineVirtualCamera _playerCamera;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ChangeCameras();
        }
    }

    private void ChangeCameras()
    {
        if (inMenu)
        {
            _menuCamera.Priority = 0;
            _playerCamera.Priority = 1;
        } else
        {
            _menuCamera.Priority = 1;
            _playerCamera.Priority = 0;
        }
        inMenu = !inMenu;

    }


}
