using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum GameState { GS_PAUSEMENU, GS_GAME, GS_LEVELCOMPLETED, GS_GAME_OVER, GS_OPTIONS, GS_CONTROLS, GS_CREDITS, GS_MAINMENU}

public class GameManager : MonoBehaviour
{
    public GameState currentGameState = GameState.GS_GAME;
    public static GameManager instance;
    public Canvas inGameCanvas;
    public Canvas gameOverCanvas;
    public Canvas controlsCanvas;
    public Canvas creditsCanvas;
    public Canvas infoCanvas;
    public TMP_Text timerText;
    private float timer;
    public TMP_Text scoreText;
    public TMP_Text infoText;
    public TMP_Text finalScoreText;
    public TMP_Text highScoreText;
    private int score = 0;
    public TMP_Text killsText;
    public TMP_Text livesText;
    public int kills = 0;
    public Image[] keysTab;
    public int keysFound = 0;
    public int lives = 3;
    public Canvas pauseMenuCanvas;
    public Canvas levelCompletedCanvas;
    public Canvas optionsCanvas;
    private const string keyHighScore = "HighScore193586";
    private const string keyHighScore2 = "HighScore193058";
    public TMP_Text qualitySettingText;
    public Slider volumeSlider;



    // Start is called before the first frame update
    void Start()
    {
       infoCanvas.enabled = false;
       volumeSlider.value = AudioListener.volume;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(currentGameState == GameState.GS_PAUSEMENU)
            {
                InGame();
            }
            else if(currentGameState == GameState.GS_GAME)
            {
                PauseMenu();
            }
            else if (currentGameState == GameState.GS_CONTROLS)
            {
                PauseMenu();
            }
            else if (currentGameState == GameState.GS_OPTIONS)
            {
                PauseMenu();
            }
            else if (currentGameState == GameState.GS_CREDITS)
            {
                PauseMenu();
            }
        }
        if(currentGameState == GameState.GS_GAME)
        {
            UpdateTime();
        }

    }

    private void UpdateTime()
    {
        timer += Time.deltaTime;
        int minutes = Mathf.FloorToInt(timer / 60F);
        int seconds = Mathf.FloorToInt(timer % 60F);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    void Awake()
    {
        instance = this;
        score = 0;
        livesText.text = lives.ToString();
        volumeSlider.value = AudioListener.volume;
        qualitySettingText.text = QualitySettings.names[QualitySettings.GetQualityLevel()];
        for (int i = 0; i < keysTab.Length ;  i++) 
        {
            keysTab[i].color = Color.grey;
        }
        InGame();
        if (!PlayerPrefs.HasKey(keyHighScore)){
            PlayerPrefs.SetInt(keyHighScore, 0);
        }
    }

    private void SetGameState(GameState newGameState)
    {
        currentGameState = newGameState;
        if (currentGameState == GameState.GS_GAME)
        {
            inGameCanvas.enabled = true;
            pauseMenuCanvas.enabled = false;
            levelCompletedCanvas.enabled = false;
            optionsCanvas.enabled = false;
            gameOverCanvas.enabled = false;
            controlsCanvas.enabled = false;
            creditsCanvas.enabled = false;
        }
        else if (currentGameState == GameState.GS_PAUSEMENU) {
            inGameCanvas.enabled = false;
            pauseMenuCanvas.enabled = true;
            levelCompletedCanvas.enabled = false;
            optionsCanvas.enabled = false;
            gameOverCanvas.enabled = false;
            controlsCanvas.enabled = false;
            creditsCanvas.enabled = false;
        }
        else if (currentGameState == GameState.GS_LEVELCOMPLETED)
        {
            levelCompletedCanvas.enabled = true;
            inGameCanvas.enabled = false;
            pauseMenuCanvas.enabled = false;
            optionsCanvas.enabled = false;
            gameOverCanvas.enabled = false;
            controlsCanvas.enabled = false;
            creditsCanvas.enabled = false;



            Scene currentScene = SceneManager.GetActiveScene();
            if (currentScene.name == "193586")
            {
                // Dodanie bonusowych pkt za czas
                int bonusPoints = 1000;
                if(bonusPoints > timer)
                {
                    bonusPoints -= (int)timer;
                    score += bonusPoints;
                    Debug.Log("Dodane pkt za czas" + bonusPoints);
                }
                int highScore = PlayerPrefs.GetInt(keyHighScore);
                if (highScore < score)
                {
                    highScore = score;
                    PlayerPrefs.SetInt(keyHighScore, highScore);
                }
                finalScoreText.text = "YOUR SCORE = " + score;
                highScoreText.text = "HIGH SCORE = " + highScore;
            }
            else if (currentScene.name == "193058")
            {
                // Dodanie bonusowych pkt za czas
                int bonusPoints = 1000;
                if (bonusPoints > timer)
                {
                    bonusPoints -= (int)timer;
                    score += bonusPoints;
                    Debug.Log("Dodane pkt za czas" + bonusPoints);
                }
                int highScore = PlayerPrefs.GetInt(keyHighScore2);
                if (highScore < score)
                {
                    highScore = score;
                    PlayerPrefs.SetInt(keyHighScore2, highScore);
                }
                finalScoreText.text = "YOUR SCORE = " + score;
                highScoreText.text = "HIGH SCORE = " + highScore;
            }
        }
        else if (currentGameState == GameState.GS_GAME_OVER)
        {
            levelCompletedCanvas.enabled = false;
            inGameCanvas.enabled = false;
            pauseMenuCanvas.enabled = false;
            optionsCanvas.enabled = false;
            gameOverCanvas.enabled = true;
            controlsCanvas.enabled = false;
            creditsCanvas.enabled = false;
        }
        else if(currentGameState == GameState.GS_OPTIONS)
        {
            levelCompletedCanvas.enabled = false;
            inGameCanvas.enabled = false;
            pauseMenuCanvas.enabled = false;
            optionsCanvas.enabled = true;
            gameOverCanvas.enabled = false;
            controlsCanvas.enabled = false;
            creditsCanvas.enabled = false;
        }
        else if (currentGameState == GameState.GS_CONTROLS)
        {
            levelCompletedCanvas.enabled = false;
            inGameCanvas.enabled = false;
            pauseMenuCanvas.enabled = false;
            optionsCanvas.enabled = false;
            gameOverCanvas.enabled = false;
            controlsCanvas.enabled = true;
            creditsCanvas.enabled = false;
        }
        else if (currentGameState == GameState.GS_CREDITS)
        {
            levelCompletedCanvas.enabled = false;
            inGameCanvas.enabled = false;
            pauseMenuCanvas.enabled = false;
            optionsCanvas.enabled = false;
            gameOverCanvas.enabled = false;
            controlsCanvas.enabled = false;
            creditsCanvas.enabled = true;
        }
    }

    public void PauseMenu()
    {
        Time.timeScale = 0f;
        SetGameState(GameState.GS_PAUSEMENU);
    }
    public void InGame()
    {
        Time.timeScale = 1f;
        SetGameState(GameState.GS_GAME);
    }
    public void LevelCompleted()
    {
        Time.timeScale = 0f;
        SetGameState(GameState.GS_LEVELCOMPLETED);
    }
    public void GameOver()
    {
        SetGameState(GameState.GS_GAME_OVER);
    }

    private IEnumerator InfoShowTime()
    {
        infoCanvas.enabled = true;
        yield return new WaitForSeconds(3.0f);
        infoCanvas.enabled = false;
    }

    public void ShowInfo()
    {
        StartCoroutine(InfoShowTime());
        
    }
    public void Options()
    {
        Time.timeScale = 0f;
        SetGameState(GameState.GS_OPTIONS);
    }

    public void Controls()
    {
        Time.timeScale = 0f;
        SetGameState(GameState.GS_CONTROLS);
    }

    public void Credits()
    {
        Time.timeScale = 0f;
        SetGameState(GameState.GS_CREDITS);
    }

    public void AddPoints(int points)
    {
        score += points;
        scoreText.text = score.ToString();
    }

    public int GetPoints()
    {
        return score;
    }

    public void AddKey(string keyColor)
    {
        if(keyColor == "Yellow key")
        {
            keysTab[0].color = Color.white;
        }
        else if (keyColor == "Green key")
        {
            keysTab[1].color = Color.green;
        }
        else if (keyColor == "Red key")
        {
            keysTab[2].color = Color.red;
        }

        this.keysFound++;
    }
    public void AddLife()
    {
        this.lives++;
        livesText.text = lives.ToString();
    }
    public void DeleteLife()
    {
        this.lives--;
        livesText.text = lives.ToString();
    }
    public void AddKills()
    {
        this.kills++;
        killsText.text = kills.ToString();
    }

    public void OnResumeButtonClicked()
    {
        InGame();
    }

    public void OnRestartButtonClicked()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OnReturnToMainMenuButtonClicked()
    {
        int sceneIndex = SceneUtility.GetBuildIndexByScenePath("Scenes/Main Menu");
        if (sceneIndex >= 0)
        {
            SceneManager.LoadSceneAsync(sceneIndex); //³adowanie sceny ³¹cz¹cej gry
        }
        else
        {
            //sceneIndex jest równe -1. Nie znaleziono sceny.
            //³adowanie innej sceny docelowo na laboratorium
            SceneManager.LoadScene("MainMenu");
        }  
    }

    public void OnSettingsButtonClicked()
    {
        Options();
    }

    public void OnControlsButtonClicked()
    {
        Controls();
    }

    public void OnCreditsButtonClicked()
    {
        Credits();
    }

    public void OnGoBackToPauseMenuButtonClicked()
    {
        PauseMenu();
    }

    public void OnQualityUpButtonClicked()
    {
        QualitySettings.IncreaseLevel();
        qualitySettingText.text = QualitySettings.names[QualitySettings.GetQualityLevel()];
    }

    public void OnQualityDownButtonClicked()
    {
        QualitySettings.DecreaseLevel();
        qualitySettingText.text = QualitySettings.names[QualitySettings.GetQualityLevel()];
    }


    public void SetVolume()
    {
        AudioListener.volume = volumeSlider.value;
    }

}
   