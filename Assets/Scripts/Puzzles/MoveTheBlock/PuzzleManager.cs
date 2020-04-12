using UnityEngine;
using UnityEngine.Events;
using Yarn.Unity;

public class PuzzleManager : MonoBehaviour
{
    public bool gameRunning;

    public UnityEvent onGameStart, onGameEnd;

    [SerializeField] public BlockSetter setter;


    [YarnCommand("StartGame")]
    public void StartGame()
    {
        setter.setBlocks();
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
