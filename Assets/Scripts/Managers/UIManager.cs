using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public enum UIState
    {
        MainMenu,
        Gameplay,
        Pause,
        Options
    }

    private UIState state;
    private UIState oldState;

    public GameObject mainMenuUI;
    public GameObject gameplayUI;
    public GameObject pauseUI;
    public GameObject optionsUI;

    private void Awake()
    {
    }

    // Start is called before the first frame update
    void Start()
    {
        ChangeState(UIState.MainMenu);     
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case UIState.MainMenu:
                mainMenuUI.SetActive(true);
                break;
            case UIState.Gameplay:
                gameplayUI.SetActive(true);
                break;
            case UIState.Pause:
                pauseUI.SetActive(true);
                break;
            case UIState.Options:
                optionsUI.SetActive(true);
                break;
            default:
                break;
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (state == UIState.Pause)
            {
                ChangeState(UIState.Gameplay);
            }
            else if (state == UIState.Gameplay)
            {
                ChangeState(UIState.Pause);
            }
        }
    }

    public void ChangeState(UIState newState)
    {
        mainMenuUI.SetActive(false);
        gameplayUI.SetActive(false);
        pauseUI.SetActive(false);
        optionsUI.SetActive(false);

        state = newState;
    }

    public void MainMenuUI()
    {
        oldState = state;
        ChangeState(UIState.MainMenu);
    }

    public void GameplayUI()
    {
        oldState = state;
        ChangeState(UIState.Gameplay);
    }

    public void PauseUI()
    {
        oldState = state;
        ChangeState(UIState.Pause);
    }

    public void OptionsUI()
    {
        oldState = state;
        ChangeState(UIState.Options);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Back()
    {
        ChangeState(oldState);
    }
}
