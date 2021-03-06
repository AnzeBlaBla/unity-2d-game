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

        inputActions.UI.Pause.performed += ctx => Pause();

        HidePauseMenu();
    }

    void Pause()
    {
        if(!DeathScreen.visible && isActiveAndEnabled && GameManager.Instance.playing)
            TogglePauseMenu();
    }

    void ShowPauseMenu()
    {
        GameManager.Instance.PauseGame();

        pauseMenu.SetActive(true);
        visible = true;
    }
    void HidePauseMenu()
    {
        GameManager.Instance.ResumeGame();

        pauseMenu.SetActive(false);
        visible = false;
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
