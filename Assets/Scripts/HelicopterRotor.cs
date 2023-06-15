using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelicopterRotor : MonoBehaviour
{
    [SerializeField] private Transform _rotor;
    [SerializeField] private Transform _tailRotor;
    [SerializeField] private float _speed;

    private void Update()
    {
        RotateRotor();
    }

    private void RotateRotor()
    {
        Vector3 rotateEuler = Vector3.up * _speed;
        Debug.Log(rotateEuler);
        // Vector3 rotateEuler = Vector3.up;

        _rotor.Rotate(rotateEuler);
        _tailRotor.Rotate(rotateEuler);
    }
}
