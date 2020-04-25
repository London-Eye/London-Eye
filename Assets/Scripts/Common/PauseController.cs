using UnityEngine;
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
            Time.timeScale = 1;
            _isPaused = value;
            ShowPauseMenu(value);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            IsPaused = !IsPaused;
            if (IsPaused)
            {
                Time.timeScale = 0;
            }
            else {
                Time.timeScale = 1;
            }
        }
    }

    private void ShowPauseMenu(bool isPaused)
    {
        pausePanel.gameObject.SetActive(isPaused);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1;
    }

    public void GoToMenu() => GoToMainMenu();
    public void GoToStart() => GoToStartMenu();
    public void ExitGame() => Exit();

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
    public static void GoToStartMenu() => SceneManager.LoadScene(0);
    public static void Exit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }
}
