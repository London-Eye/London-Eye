﻿using UnityEngine;
using UnityEngine.SceneManagement;
using Yarn.Unity;

public class PauseController : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel;

    private bool _isPaused;
    public bool IsPaused
    {
        get => _isPaused;
        set
        {
            _isPaused = value;
            Time.timeScale = value ? 0f : 1f;
            ShowPauseMenu(value);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            IsPaused = !IsPaused;
        }
    }

    private void ShowPauseMenu(bool isPaused)
    {
        pausePanel.gameObject.SetActive(isPaused);
    }

    public void Restart()
    {
        IsPaused = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void RestartGame()
    {
        IsPaused = false;
        DeleteGame();
        StartGame();
    }

    public void GoToMenu() => GoToMainMenu();
    public void GoToStart() => GoToStartMenu();
    public void ExitGame()
    {
        IsPaused = false;
        Exit();
    }

    public static void GoToMainMenu()
    {
        Time.timeScale = 1;
        AsyncOperation loadSceneOperation = SceneManager.LoadSceneAsync(1); // Load Main Menu
        loadSceneOperation.completed += op =>
        {
            DialogueRunner dialogueRunner = FindObjectOfType<DialogueRunner>();
            dialogueRunner.startNode = "MainMenu-Back";
            dialogueRunner.startAutomatically = true;
        };
    }
    public static void GoToStartMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    public static void DeleteGame()
    {
        if (CharacterCreation.Instance != null)
        {
            Destroy(CharacterCreation.Instance.gameObject);
        }
    }

    public static void StartGame() => SceneManager.LoadScene(1);
    public static void Exit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }
}
