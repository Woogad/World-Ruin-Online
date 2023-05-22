using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

public class TestingConnectionUI : MonoBehaviour
{
    [SerializeField] private Button _hostBn;
    [SerializeField] private Button _clientBn;

    private void Awake()
    {
        _hostBn.onClick.AddListener(() =>
        {
            GameMultiplayer.Instance.StartHost();
            Hide();
        });
        _clientBn.onClick.AddListener(() =>
        {
            GameMultiplayer.Instance.StartClient();
            Hide();
        });
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
