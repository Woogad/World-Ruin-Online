using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoaderCallback : MonoBehaviour
{
    private bool _isLoad = true;
    private void Update()
    {
        if (_isLoad)
        {
            _isLoad = false;
            Loader.LoaderCallback();
        }
    }
}
