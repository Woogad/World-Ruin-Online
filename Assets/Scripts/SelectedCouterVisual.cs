using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedCouterVisual : MonoBehaviour
{
    [SerializeField] private ClearCounter _clearCounter;
    [SerializeField] private GameObject _visualGameObject;
    private void Start()
    {
        Player.Instance.OnSelectedCounterChanged += PlayerOnSelectedCounterChanged;
    }

    private void PlayerOnSelectedCounterChanged(object sender, Player.OnSelectedCoutnerChangedEventArgs e)
    {
        if (e.SelectedCounter == _clearCounter)
        {
            Show();
        }
        else
        {
            Hide();
        }

    }

    private void Show()
    {
        _visualGameObject.SetActive(true);
    }
    private void Hide()
    {
        _visualGameObject.SetActive(false);
    }
}
