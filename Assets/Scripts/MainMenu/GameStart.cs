using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStart : MonoBehaviour
{
    private void Start()
    {
        DeleteGame();
    }

    public static void DeleteGame()
    {
        if (CharacterCreation.Instance != null)
        {
            Destroy(CharacterCreation.Instance.gameObject);
        }
    }

    public void StartGame() => SceneManager.LoadScene(1);
    public void ExitGame() => PauseController.Exit();
}
