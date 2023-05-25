using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreBoadUI : MonoBehaviour
{
    [SerializeField] private Transform _scoreBoardItemTemplate;
    [SerializeField] private Transform _contrainer;

    private List<ScoreBoardItemUI> _scoreBoardItemTemplateList = new List<ScoreBoardItemUI>();

    private void Start()
    {
        KillScoreManager.Instance.OnScoreBoardPlayersChanged += KillScoreManagerOnScoreBoardPlayerChanged;
        GameInput.Instance.OnViewScoreBoardHoldAction += GameInputOnViewScoreBoardHoldAction;
        Hide();
    }

    private void GameInputOnViewScoreBoardHoldAction(object sender, GameInput.OnViewScoreBoardHoldActionArgs e)
    {
        if (e.IsHoldViewScoreBoardAction)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    private void KillScoreManagerOnScoreBoardPlayerChanged(object sender, KillScoreManager.OnScoreBoardPlayersChangedArgs e)
    {
        if (e.IsInit)
        {
            CreateVisualScoreBoard(e.ScoreBoardDictionary);
        }
        else
        {
            UpdateVisualKillScore(e.ScoreBoardDictionary);
        }
    }

    private void CreateVisualScoreBoard(Dictionary<ulong, ScoreBoardStruct> scoreBoardDictionary)
    {

        foreach (var kvp in scoreBoardDictionary)
        {
            ScoreBoardItemUI item = Instantiate(_scoreBoardItemTemplate, _contrainer).GetComponent<ScoreBoardItemUI>();
            item.UsernameText.text = kvp.Value.Username.ToString();
            item.KillScoreText.text = kvp.Value.KillScore.ToString();

            _scoreBoardItemTemplateList.Add(item);
        }
    }

    private void UpdateVisualKillScore(Dictionary<ulong, ScoreBoardStruct> scoreBoardDictionary)
    {
        foreach (var kvp in scoreBoardDictionary)
        {
            _scoreBoardItemTemplateList[(int)kvp.Key].KillScoreText.text = kvp.Value.KillScore.ToString();
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
