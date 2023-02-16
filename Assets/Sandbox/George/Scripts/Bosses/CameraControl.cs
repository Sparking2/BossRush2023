using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public int bossIndex;
    public void StartBlend()
    {
        CameraManager.Instance.TransitionToBossCameras(bossIndex);
    }

    public void ReturnToPlayer()
    {
        CameraManager.Instance.ReturnCameraToPlayer();
    }
}
