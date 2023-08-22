using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;

public class FollowPlayerCamera : MonoBehaviour
{
    private Transform _playerTransform;
    private CinemachineVirtualCamera _cinemachineVirtualCamera;

    private void Start()
    {
        _cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
        if (Player.LocalInstance != null)
        {
            SetupCamera();
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
            SetupCamera();
        }
    }

    public void SetupCamera()
    {
        _playerTransform = Player.LocalInstance.GetComponent<Transform>();
        _cinemachineVirtualCamera.Follow = _playerTransform;
        _cinemachineVirtualCamera.LookAt = _playerTransform;
    }

}
