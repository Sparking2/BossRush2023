using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Player;

public class CameraManager : MonoBehaviour
{
    private bool inMenu = true;
    public static CameraManager Instance { get; private set; }
    [SerializeField]
    private CinemachineVirtualCamera _menuCamera;
    [SerializeField]
    private CinemachineVirtualCamera _playerCamera;
    [SerializeField] private CinemachineVirtualCamera[] _bossCameras;

    private ComponentInput _componentInput;
    private ComponentAnimator _componentAnimator;
    private BossManager _bossManager;

    private void Awake()
    {
        _bossManager = GameObject.FindGameObjectWithTag("BossManager").GetComponent<BossManager>();
        _componentAnimator = GameObject.FindGameObjectWithTag("Player").GetComponent<ComponentAnimator>();
        _componentInput = GameObject.Find("Player").GetComponent<ComponentInput>();
        Instance = this;
    }

    private void Start()
    {
        _componentInput.enabled = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && inMenu)
        {
            ChangeMenuCamera();
        }
    }

    private void ChangeMenuCamera()
    {
        _componentInput.enabled = true;
            _menuCamera.Priority = 0;
            _playerCamera.Priority = 1;
        inMenu = false;
        _componentAnimator.Activate();
        _bossManager.OnGameStart();
    }
    public void TransitionToBossCameras(int _camIndex)
    {
        _componentInput.enabled = false;
        _playerCamera.Priority = 0;
        _menuCamera.Priority = 0;
        for (int i=0;i < _bossCameras.Length; i++)
        {
            _bossCameras[i].Priority = 0;
        }

        _bossCameras[_camIndex].Priority = 1;
    }

    public void ReturnCameraToPlayer()
    {
        _componentInput.enabled = true;
        _playerCamera.Priority = 1;
        for (int i = 0; i < _bossCameras.Length; i++)
        {
            _bossCameras[i].Priority = 0;
        }
    }

}
