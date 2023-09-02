using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.Netcode;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _winerUsernameText;
    [SerializeField] private TextMeshProUGUI _scoreCountText;
    [SerializeField] private TextMeshProUGUI _winerHeaderText;
    [SerializeField] private Button _mainMenuBn;

    private string _winerUsername;
    private int _maxScoreCount;
    private List<ScoreBoardStruct> _scoreSameValue = new List<ScoreBoardStruct>();

    private void Awake()
    {
        _mainMenuBn.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.Shutdown();
            Loader.Load(Loader.Scene.MainMenuScene);
        });
    }

    private void Start()
    {
        GameManager.Instance.OnStateChanged += GameManagerOnStateChanged;
        Hide();
    }

    private void GameManagerOnStateChanged(object sender, EventArgs e)
    {
        if (GameManager.Instance.IsGameOver())
        {
            UpdateVisual();
            Show();
        }
        else
        {
            Hide();
        }
    }

    private void UpdateVisual()
    {
        foreach (var kvp in ScoreBoardManager.Instance.GetScoreBoardDictionary())
        //* Check most score.
        {
            if (kvp.Value.KillScore < _maxScoreCount) continue;
            if (kvp.Value.KillScore == _maxScoreCount)
            {
                _scoreSameValue.Add(kvp.Value);
                continue;
            }
            else
            {
                _winerUsername = kvp.Value.Username.ToString();
                _maxScoreCount = kvp.Value.KillScore;
            }
        }

        if (_scoreSameValue.Count != 0)
        //* If there have same score.
        {
            foreach (var item in _scoreSameValue)
            {
                if (item.KillScore != _maxScoreCount) continue;
                _winerHeaderText.text = "Draw";
                _winerUsername = "---";
            }
        }
        _winerUsernameText.text = _winerUsername;
        _scoreCountText.text = _maxScoreCount.ToString();

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
