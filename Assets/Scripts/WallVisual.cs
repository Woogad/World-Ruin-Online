using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallVisual : MonoBehaviour
{
    [SerializeField] private GameObject _wallVisual;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Player>(out Player player))
        {
            if (player.OwnerClientId == Player.LocalInstance.OwnerClientId)
            {
                ShowWall();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<Player>(out Player player))
        {
            if (player.OwnerClientId == Player.LocalInstance.OwnerClientId)
            {
                HideWall();
            }
        }
    }

    private void ShowWall()
    {

        _wallVisual.SetActive(true);
    }
    private void HideWall()
    {

        _wallVisual.SetActive(false);
    }
}
