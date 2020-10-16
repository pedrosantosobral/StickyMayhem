using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomEventSystem;

public class GameManager : MonoBehaviour
{
    [SerializeField] private VoidEvent startGame;
    [SerializeField] private VoidEvent stopGame;
    [SerializeField] private VoidEvent pauseGame;
    [SerializeField] private VoidEvent continueGame;

    [SerializeField] private VoidEvent spawnPlayer;
   
    public bool _startGame;
    public bool _stopGame;

    private bool _isPlaying = false;
    public void StartGame()
    {
        Time.timeScale = 1;
        startGame.Raise();
        spawnPlayer.Raise();
        _isPlaying = true;
    }

    public void StopGame()
    {
        stopGame.Raise();
        _isPlaying = false;
        Time.timeScale = 1;
    }

    public void PauseGame()
    {
        if(_isPlaying == true)
        {
            pauseGame.Raise();
            Time.timeScale = 0;
        }
    }

    public void ContinueGame()
    {
        if (_isPlaying == true)
        {
            Time.timeScale = 1;
            continueGame.Raise();
        }
    }

    private void Update()
    {
        if (_startGame == true)
        {
            StartGame();
            _startGame = false;
        }

        if (_stopGame == true)
        {
            StopGame();
            _stopGame = false;
        }
    }


}
