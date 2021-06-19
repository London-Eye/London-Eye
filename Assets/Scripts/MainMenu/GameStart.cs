using UnityEngine;

public class GameStart : MonoBehaviour
{
    private void Start() => PauseController.DeleteGame();
    public void StartGame() => PauseController.StartGame();
    public void ExitGame() => PauseController.Exit();
}
