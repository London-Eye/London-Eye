using UnityEngine;
using UnityEngine.SceneManagement;

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
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    [Yarn.Unity.YarnCommand("BackToMainMenu")]
    public void GoToMenu() => SceneManager.LoadScene(0);
}
