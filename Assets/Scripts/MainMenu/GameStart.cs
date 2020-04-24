using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStart : MonoBehaviour
{
    public void StartGame() {
        SceneManager.LoadScene("MainMenu");
    }
    public void ExitGame() => PauseController.Exit();
}
