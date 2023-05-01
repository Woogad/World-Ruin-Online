using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUI : MonoBehaviour
{
    [SerializeField] private Image _barImage;
    //TODO https://youtu.be/AmGSEH7QcDg?t=18085 

    private void Start()
    {
        Player.Instance.OnReloadProgressChanged += PlayerOnReloadProgress;
        _barImage.fillAmount = 0f;
        Hide();
    }

    private void PlayerOnReloadProgress(object sender, Player.OnReloadProgressChangedArgs e)
    {
        _barImage.fillAmount = e.ReloadProgressNormalized;
        if (_barImage.fillAmount == 0f || _barImage.fillAmount == 1f)
        {
            Hide();
        }
        else
        {
            Show();
        }
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }
    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
