using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;
using UnityEngine.Video;


public class MainMenu : MonoBehaviour
{
    public GameState currentGameState = GameState.GS_MAINMENU;
    public RawImage rawImage;
    public VideoPlayer videoPlayer;
    public Canvas controlsCanvas;
    public Canvas creditsCanvas;
    public Canvas optionsCanvas;
    public TMP_Text qualitySettingText;
    public Slider volumeSlider;

    // Start is called before the first frame update
    void Start()
    {
        volumeSlider.value = AudioListener.volume;
        qualitySettingText.text = QualitySettings.names[QualitySettings.GetQualityLevel()];
    }


    

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (currentGameState == GameState.GS_CONTROLS)
            {
                InMainMenu();
            }
            else if (currentGameState == GameState.GS_OPTIONS)
            {
                InMainMenu();
            }
            else if (currentGameState == GameState.GS_CREDITS)
            {
                InMainMenu();
            }
        }

    }

    public void OnLevel1ButtonPressed()
    {
        SceneManager.LoadSceneAsync("193586");
    }

    public void OnLevel2ButtonPressed()
    {
        SceneManager.LoadSceneAsync("193058");
    }

    public void OnExitToDesktopButtonPressed()
    {
    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    #endif
        Application.Quit();
    }

    private void SetGameState(GameState newGameState)
    {
        currentGameState = newGameState;
        if (currentGameState == GameState.GS_MAINMENU)
        {
            optionsCanvas.enabled = false;
            controlsCanvas.enabled = false;
            creditsCanvas.enabled = false;
        }
        else if (currentGameState == GameState.GS_OPTIONS)
        {
            optionsCanvas.enabled = true;
            controlsCanvas.enabled = false;
            creditsCanvas.enabled = false;
        }
        else if (currentGameState == GameState.GS_CONTROLS)
        {
            optionsCanvas.enabled = false;
            controlsCanvas.enabled = true;
            creditsCanvas.enabled = false;
        }
        else if (currentGameState == GameState.GS_CREDITS)
        {
            optionsCanvas.enabled = false;
            controlsCanvas.enabled = false;
            creditsCanvas.enabled = true;
        }
    }

    public void Options()
    {
        SetGameState(GameState.GS_OPTIONS);
    }

    public void Controls()
    {
        SetGameState(GameState.GS_CONTROLS);
    }

    public void Credits()
    {
        SetGameState(GameState.GS_CREDITS);
    }

    public void InMainMenu()
    {
        SetGameState(GameState.GS_MAINMENU);
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
