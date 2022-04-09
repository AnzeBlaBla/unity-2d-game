using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuController : MonoBehaviour
{
    public GameObject pauseMenu;
    InputActions inputActions;
    bool visible = true;

    void Start()
    {
        inputActions = InputController.Instance.inputActions;

        inputActions.Player.Pause.performed += ctx => Pause();

        HidePauseMenu();
    }

    void Pause()
    {
        if(!DeathScreen.visible && isActiveAndEnabled && GameManager.Instance.playing)
            TogglePauseMenu();
    }

    void ShowPauseMenu()
    {
        AudioManager.Instance.PauseAllSounds();
        EnemyController.PauseAllSounds();

        pauseMenu.SetActive(true);
        visible = true;
        Time.timeScale = 0;
    }
    void HidePauseMenu()
    {
        AudioManager.Instance.ResumeAllSounds();
        EnemyController.ResumeAllSounds();

        pauseMenu.SetActive(false);
        visible = false;
        Time.timeScale = 1;
    }

    void TogglePauseMenu()
    {
        if (visible)
        {
            HidePauseMenu();
        }
        else
        {
            ShowPauseMenu();
        }
    }


    public void Resume()
    {
        HidePauseMenu();
    }

    public void MainMenu()
    {
        HidePauseMenu();
        GameManager.Instance.MainMenu();
    }
}
