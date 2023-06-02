using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestingReadyUI : MonoBehaviour
{
    [SerializeField] Button _readyBn;

    private void Awake()
    {
        _readyBn.onClick.AddListener(() =>
        {
            ChareacterSelectReady.Instance.SetPlayerReady();
        });
    }
}
