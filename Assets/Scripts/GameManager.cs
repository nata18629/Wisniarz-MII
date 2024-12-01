using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;



public class GameManager : MonoBehaviour
{

    public Canvas pauseMenuCanvas;
    public Canvas inGameCanvas;
    public enum GameState { GAME, PAUSE_MENU, LEVEL_COMPLETED, GAME_OVER }
    public GameState currentGameState = GameState.PAUSE_MENU;
    static public GameManager instance;
    public TMP_Text scoreText;
    int score = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            if(currentGameState== GameState.PAUSE_MENU)
            {
                InGame();
            }
            else if(currentGameState == GameState.GAME)
            {
                PauseMenu();
            }
        }
    }

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogError("Duplicated Game Manager", gameObject);
        }
    }

    public void AddPoints(int points)
    {
        score += points;
        scoreText.text = score.ToString();
    }

    void SetGameState(GameState newGameState)
    {
        pauseMenuCanvas.enabled = false;
        inGameCanvas.enabled = false;
        currentGameState = newGameState;
    }
    public void PauseMenu()
    {
        SetGameState(GameState.PAUSE_MENU);
        pauseMenuCanvas.enabled = true;
    }

    public void InGame()
    {
        SetGameState(GameState.GAME);
        inGameCanvas.enabled = true;
    }
    public void LevelCompleted()
    {
        SetGameState(GameState.LEVEL_COMPLETED);
    }
    public void GameOver()
    {
        SetGameState(GameState.GAME_OVER);
    }
    
}
