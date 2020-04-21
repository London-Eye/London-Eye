using UnityEngine;

public class GameStart : MonoBehaviour
{
    public void StartGame() => PauseController.GoToMainMenu();
    public void ExitGame() => PauseController.Exit();
}
