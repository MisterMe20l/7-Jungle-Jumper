using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GamePause : MonoBehaviour
{
    [SerializeField] Player player1;

    GameInput gameInput;
    InputAction pause_k;
    InputAction pause_g;
    InputAction resume;

    bool paused;

    private void Awake()
    {
        gameInput = new GameInput();
    }

    private void OnEnable()
    {
        pause_k = gameInput.Menu.PauseKeyboad;
        pause_k.Enable();
        pause_k.performed += OnPauseInput;

        pause_g = gameInput.Menu.PauseGamepad;
        pause_g.Enable();
        pause_g.performed += OnPauseInput;
        pause_g.performed += OnReturnInput;
    }

    private void OnDisable()
    {
        pause_k.Disable();
        pause_g.Disable();
    }

    private void Update()
    {
        paused = IsPaused();
        player1.canShoot = !paused;
    }

    private void OnPauseInput(InputAction.CallbackContext context)
    {
        if (!paused) // Pause
        {
            SceneLoader.instance.LoadScene("Pause Menu", UnityEngine.SceneManagement.LoadSceneMode.Additive);
        }
    }

    private void OnReturnInput(InputAction.CallbackContext context)
    {
        if (paused && SceneManager.loadedSceneCount != 3) // Unpause
        {
            Time.timeScale = 1.0f;
            SceneLoader.instance.UnloadSceneAsync("Pause Menu");
        }
    }

    private bool IsPaused()
    {
        return Time.timeScale == 0;
    }
}
