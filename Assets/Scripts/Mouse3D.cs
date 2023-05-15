using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mouse3D : MonoBehaviour
{
    private Camera _cam;
    private const string CamNameTag = "MainCamera";

    [SerializeField] private LayerMask _layerMask;

    private void Start()
    {
        _cam = GameObject.FindGameObjectWithTag(CamNameTag).GetComponent<Camera>();
        GameInput.Instance.OnEscAction += GameInputOnEscAcion;
    }

    private void GameInputOnEscAcion(object sender, GameInput.OnEscActionArgs e)
    {
        if (e.IsEscMenuOpen)
        {
            Hide();
        }
        else
        {
            Show();
        }
    }

    private void Update()
    {
        Ray ray = _cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit rayHit, float.MaxValue, _layerMask))
        {
            transform.position = rayHit.point;
        }
    }

    public Transform GetAngleAimRotateTransform()
    {
        Transform MousePosition = transform;

        return MousePosition;
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }
    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
