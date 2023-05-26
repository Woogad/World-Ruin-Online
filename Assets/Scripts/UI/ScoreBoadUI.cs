using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreBoadUI : MonoBehaviour
{
    [SerializeField] private Transform _scoreBoardItemTemplate;
    [SerializeField] private Transform _contrainer;

    private Dictionary<ulong, ScoreBoardItemUI> _scoreBoardItemDictionary = new Dictionary<ulong, ScoreBoardItemUI>();

    private void Start()
    {
        ScoreBoardManager.Instance.OnScoreBoardPlayersCreate += ScoreBoardManagerOnScoreBoardPlayerCreate;
        ScoreBoardManager.Instance.OnDeleteScoreBoardItem += ScoreBoardManagerOnDeleteScoreBoardItem;
        ScoreBoardManager.Instance.OnScoreBoardKillChanged += ScoreBoardManagerOnScoreBoardKillChanged;
        GameInput.Instance.OnViewScoreBoardHoldAction += GameInputOnViewScoreBoardHoldAction;
        Hide();
    }

    private void ScoreBoardManagerOnScoreBoardKillChanged(object sender, ScoreBoardManager.OnScoreBoardScoreChangedArgs e)
    {
        ScoreBoardItemUI item = _scoreBoardItemDictionary[e.ClientID].GetComponent<ScoreBoardItemUI>();
        item.KillScoreText.text = e.Value.ToString();
        _scoreBoardItemDictionary[e.ClientID] = item;
    }

    private void ScoreBoardManagerOnDeleteScoreBoardItem(object sender, ScoreBoardManager.OnScoreBoardScoreChangedArgs e)
    {
        Destroy(_scoreBoardItemDictionary[e.ClientID].gameObject);
        _scoreBoardItemDictionary.Remove(e.ClientID);
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

    private void ScoreBoardManagerOnScoreBoardPlayerCreate(object sender, ScoreBoardManager.OnScoreBoardPlayersCreateArgs e)
    {
        CreateVisualScoreBoard(e.ScoreBoardDictionary);
    }

    private void CreateVisualScoreBoard(Dictionary<ulong, ScoreBoardStruct> scoreBoardDictionary)
    {

        foreach (var kvp in scoreBoardDictionary)
        {
            ScoreBoardItemUI item = Instantiate(_scoreBoardItemTemplate, _contrainer).GetComponent<ScoreBoardItemUI>();
            item.UsernameText.text = "Player " + kvp.Value.Username.ToString();
            item.KillScoreText.text = kvp.Value.KillScore.ToString();

            _scoreBoardItemDictionary[kvp.Key] = item;
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
