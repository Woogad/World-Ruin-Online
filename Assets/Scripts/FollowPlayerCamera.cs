using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;

public class FollowPlayerCamera : MonoBehaviour
{
    public static FollowPlayerCamera Instance { get; private set; }
    private Transform _playerTransform;
    private CinemachineVirtualCamera _cinemachineVirtualCamera;

    private void Start()
    {
        Instance = this;
        _cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
        if (Player.LocalInstance != null)
        {
            CameraFollow();
        }
        else
        {
            Player.OnAnyPlayerSpawned += PlayerOnAnyPlayerSpawned;
        }
    }

    private void PlayerOnAnyPlayerSpawned(object sender, EventArgs e)
    {
        if (Player.LocalInstance != null)
        {
            CameraFollow();
        }
    }


    public void CameraFollow()
    {
        _playerTransform = Player.LocalInstance.GetComponent<Transform>();
        _cinemachineVirtualCamera.Follow = _playerTransform;
        _cinemachineVirtualCamera.LookAt = _playerTransform;
    }
}
