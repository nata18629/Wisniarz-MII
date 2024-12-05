using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using TMPro;



public class GameManager : MonoBehaviour
{

    public Canvas pauseMenuCanvas;
    public Canvas gameOverCanvas;
    public Canvas inGameCanvas;
    public Canvas levelCompletedCanvas;
    public Canvas optionsCanvas;
    public enum GameState { GAME, PAUSE_MENU, LEVEL_COMPLETED, GAME_OVER, OPTIONS }
    public GameState currentGameState = GameState.PAUSE_MENU;
    static public GameManager instance;
    public TMP_Text scoreText;
    public TMP_Text endScoreText;
    public TMP_Text highScoreText;
    public TMP_Text timeText;
    public TMP_Text enemiesKilledText;
    public TMP_Text currentQualityName;

    static string keyHighScore = "HighScoreLevel1";

    public Image[] keysTab;
    public Image[] livesTab;
    int keys_found = 0;
    int lives_left = 3;
    int score = 0;
    int enemies_killed = 0;
    float timer = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
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
        timer += Time.deltaTime;
        int minutes = Mathf.FloorToInt(timer / 60); // Integer division for minutes
        int seconds = Mathf.FloorToInt(timer % 60);
        timeText.text=string.Format("{0:00}:{1:00}",minutes,seconds);
    }

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
            for(int i=0; i<keys_found; i++)
            {
                keysTab[i].color=Color.grey;
            }
            InGame();
            livesTab[3].enabled = false;
            enemiesKilledText.text = enemies_killed.ToString();
            currentQualityName.text = QualitySettings.names[QualitySettings.GetQualityLevel()];

            if (!PlayerPrefs.HasKey("HighScoreLevel1"))
            {
                PlayerPrefs.SetInt("HighScoreLevel1",0);
            }
            optionsCanvas.enabled = false;
            gameOverCanvas.enabled = false;
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

    public void AddKeys(string name)
    {
        switch (name)
        {
            case "gem R":
                keysTab[keys_found].color = Color.red;
                break;
            case "gem G":
                keysTab[keys_found].color = Color.green;
                break;
            case "gem B":
                keysTab[keys_found].color = Color.blue;
                break;
        }
        keys_found++;
    }

    public void AddEnemyKilled()
    {
        enemies_killed++;
        enemiesKilledText.text = enemies_killed.ToString();
    }

    public void AddLife()
    {
        lives_left++;
        livesTab[lives_left - 1].enabled = true;
    }
    public void LoseLife()
    {
        livesTab[lives_left - 1].enabled = false;
        lives_left--;
        if (lives_left == 0)
        {
            GameOver();
        }
    }

    void SetGameState(GameState newGameState)
    {
        if (currentGameState == GameState.OPTIONS)
        {
            optionsCanvas.enabled = false;
            Time.timeScale = 1;

        }
        if(currentGameState == GameState.PAUSE_MENU)
        {
            pauseMenuCanvas.enabled = false;
            Time.timeScale = 1;
        }
        if(currentGameState == GameState.GAME_OVER)
        {
            gameOverCanvas.enabled = false;
            Time.timeScale = 1;
        }

        if (newGameState == GameState.LEVEL_COMPLETED)
        {
            levelCompletedCanvas.enabled = true;
            Scene currentScene = SceneManager.GetActiveScene();
            if(currentScene.name=="Level1") {
                int highScore = PlayerPrefs.GetInt("HighScoreLevel1");
                if(highScore < score)
                {
                    highScore = score;
                    PlayerPrefs.SetInt("HighScoreLevel1", highScore);
                }
                endScoreText.text = "Your score = "+score.ToString();
                highScoreText.text = "The best score = "+highScore.ToString();
            }
            Time.timeScale = 0;
        }
        else
        {
            levelCompletedCanvas.enabled = false;
        }

        inGameCanvas.enabled = false;
        currentGameState = newGameState;
    }
    public void PauseMenu()
    {
        SetGameState(GameState.PAUSE_MENU);
        pauseMenuCanvas.enabled = true;
        Time.timeScale = 0;
    }

    public void InGame()
    {
        SetGameState(GameState.GAME);
        inGameCanvas.enabled = true;
    }
    public void LevelCompleted()
    {
        if (keys_found == 3)
        {
            score += lives_left * 100;
            SetGameState(GameState.LEVEL_COMPLETED);
        }
        else
        {
            Debug.Log("Not all keys have been found.");
        }
        
    }
    public void Options()
    {
        SetGameState(GameState.OPTIONS);
        optionsCanvas.enabled = true;
        Time.timeScale = 0;
    }
    public void GameOver()
    {
        SetGameState(GameState.GAME_OVER);
        gameOverCanvas.enabled = true;
        Time.timeScale = 0;
    }

    public void OnResumeButtonClicked()
    {
        InGame();
    }
    public void OnOptionsButtonClicked()
    {
        Options();
    }
    public void OnRestartButtonClicked()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void OnIncreaseButtonPressed()
    {
        int currentQualityLevel = QualitySettings.GetQualityLevel();
        int maxQualityLevel = QualitySettings.names.Length - 1;
        if (currentQualityLevel < maxQualityLevel)
        {
            QualitySettings.IncreaseLevel(true);
            currentQualityName.text = QualitySettings.names[QualitySettings.GetQualityLevel()];
        }
    }
    public void OnDecreaseButtonPressed()
    {
        int currentQualityLevel = QualitySettings.GetQualityLevel();

        if (currentQualityLevel > 0)
        {
            QualitySettings.DecreaseLevel(true);
            currentQualityName.text = QualitySettings.names[QualitySettings.GetQualityLevel()];
        }

    }
    public void SetVolume(float volume)
    {
        AudioListener.volume = volume;
    }
    public void OnReturnToMainMenuButtonClicked()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }
    
}
