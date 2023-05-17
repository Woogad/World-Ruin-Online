using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedCouterVisual : MonoBehaviour
{
    [SerializeField] private BaseCounter _baseCounter;
    [SerializeField] private GameObject[] _visualGameObjectList;
    private void Start()
    {
        // Player.Instance.OnSelectedCounterChanged += PlayerOnSelectedCounterChanged;
    }

    private void PlayerOnSelectedCounterChanged(object sender, Player.OnSelectedCoutnerChangedEventArgs e)
    {
        if (e.SelectedCounter == _baseCounter)
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
        foreach (GameObject visualGameObject in _visualGameObjectList)
        {
            visualGameObject.SetActive(true);
        }
    }
    private void Hide()
    {
        foreach (GameObject visualGameObject in _visualGameObjectList)
        {
            visualGameObject.SetActive(false);
        }
    }
}
