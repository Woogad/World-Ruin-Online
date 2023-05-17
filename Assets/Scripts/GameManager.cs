using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    private enum State
    {
        WaitingToStart,
        CountdonwToStart,
        GamePlaying,
        GameOver
    }

    public event EventHandler OnStateChanged;

    private State _state;
    private float _waitingToStartTimer = 1f;
    private float _countdownToStartTimer = 1f;
    private float _gamePlayingTimer;
    private float _gamePlayingTimerMax = 200f;
    private void Awake()
    {
        Instance = this;
        _state = State.WaitingToStart;
    }

    private void Start()
    {
        //! for testing
        _state = State.CountdonwToStart;
        OnStateChanged?.Invoke(this, EventArgs.Empty);
        //!
    }

    private void Update()
    {
        switch (_state)
        {
            case State.WaitingToStart:
                _waitingToStartTimer -= Time.deltaTime;
                if (_waitingToStartTimer < 0)
                {
                    _state = State.CountdonwToStart;
                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                }
                break;
            case State.CountdonwToStart:
                _countdownToStartTimer -= Time.deltaTime;
                if (_countdownToStartTimer < 0)
                {
                    _state = State.GamePlaying;
                    _gamePlayingTimer = _gamePlayingTimerMax;
                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                }
                break;
            case State.GamePlaying:
                _gamePlayingTimer -= Time.deltaTime;
                if (_gamePlayingTimer < 0)
                {
                    _state = State.GameOver;
                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                }
                break;
            case State.GameOver:
                break;
        }
    }

    public bool IsGamePlaying()
    {
        return _state == State.GamePlaying;
    }

    public bool IsCountdownActive()
    {
        return _state == State.CountdonwToStart;
    }

    public float GetCountdownToStartTimer()
    {
        return this._countdownToStartTimer;
    }

    public bool IsGameOver()
    {
        return _state == State.GameOver;
    }

    public float GetGamePlayingTimerNormalized()
    {
        return 1 - (_gamePlayingTimer / _gamePlayingTimerMax);
    }
}
