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
        ScoreBoardManager.Instance.OnDeleteScoreBoardItem += ScoreBoardManagerOnDeleteScoreBoardItem;
        ScoreBoardManager.Instance.OnScoreBoardChanged += ScoreBoardManagerOnScoreBoardChanged;
        GameManager.Instance.OnStateChanged += GameManagerOnStateChanged;
        GameInput.Instance.OnViewScoreBoardHoldAction += GameInputOnViewScoreBoardHoldAction;
        Hide();
    }

    private void GameManagerOnStateChanged(object sender, EventArgs e)
    {
        if (GameManager.Instance.IsCountdownToStartActive())
        {
            CreateVisualScoreBoard(ScoreBoardManager.Instance.GetScoreBoardDictionary());
        }
    }

    private void ScoreBoardManagerOnScoreBoardChanged(object sender, ScoreBoardManager.OnScoreBoardChangedArgs e)
    {
        ScoreBoardItemUI item = _scoreBoardItemDictionary[e.ClientID].GetComponent<ScoreBoardItemUI>();
        item.ScoreText.text = e.Value.ToString();
        _scoreBoardItemDictionary[e.ClientID] = item;
    }

    private void ScoreBoardManagerOnDeleteScoreBoardItem(object sender, ScoreBoardManager.OnScoreBoardChangedArgs e)
    {
        if (_scoreBoardItemDictionary.Count != 0)
        {
            Destroy(_scoreBoardItemDictionary[e.ClientID].gameObject);
            _scoreBoardItemDictionary.Remove(e.ClientID);
        }
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

    private void CreateVisualScoreBoard(Dictionary<ulong, ScoreBoardStruct> scoreBoardDictionary)
    {

        foreach (var kvp in scoreBoardDictionary)
        {
            ScoreBoardItemUI item = Instantiate(_scoreBoardItemTemplate, _contrainer).GetComponent<ScoreBoardItemUI>();
            item.UsernameText.text = kvp.Value.Username.ToString();
            item.ScoreText.text = kvp.Value.KillScore.ToString();
            item.OwnerIcon.gameObject.SetActive(kvp.Key == Player.LocalInstance.OwnerClientId);
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
