using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowControllerUI : MonoBehaviour
{
    public static ShowControllerUI Instance { get; private set; }

    [SerializeField] private Button _closeBn;

    private void Awake()
    {
        Instance = this;
        _closeBn.onClick.AddListener(() =>
        {
            Hide();
        });
    }
    private void Start()
    {
        Hide();
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
