using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunMuzzleFlashParticle : MonoBehaviour
{
    [SerializeField] private ParticleSystem _muzzleFlashParticle;
    [SerializeField] private GunObject _gunObject;

    private void Start()
    {
        _gunObject.OnShoot += GunObjectOnShoot;
    }

    private void GunObjectOnShoot(object sender, EventArgs e)
    {
        _muzzleFlashParticle.Play();
    }
}
