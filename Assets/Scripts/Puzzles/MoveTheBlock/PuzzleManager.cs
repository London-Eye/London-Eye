using UnityEngine;
using UnityEngine.Events;
using Yarn.Unity;

public class PuzzleManager : MonoBehaviour
{
    public bool gameRunning;

    public UnityEvent onGameStart, onGameEnd;

    [YarnCommand("StartGame")]
    public void StartGame()
    {
        gameRunning = true;
        onGameStart.Invoke();
    }

    [YarnCommand("EndGame")]
    public void EndGame()
    {
        gameRunning = false;
        onGameEnd.Invoke();
    }
}
