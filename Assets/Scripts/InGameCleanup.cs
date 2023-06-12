using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameCleanup : MonoBehaviour
{
    private void Awake()
    {
        if (MusicMenu.Instance != null)
        {
            Destroy(MusicMenu.Instance.gameObject);
        }
    }
}
