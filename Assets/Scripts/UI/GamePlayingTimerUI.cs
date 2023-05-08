using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayingTimerUI : MonoBehaviour
{
    [SerializeField] private Image TimerImage;

    void Update()
    {
        TimerImage.fillAmount = GameManager.Instance.GetGamePlayingTimerNormalized();
    }
}
