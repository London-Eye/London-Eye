using Assets.Scripts.Common;
using System.Collections.Generic;
using UnityEngine;

public class PoolPuzzleLoader : MonoBehaviour
{
    public List<string> puzzles;

    private SelectorPool<string> puzzlePool;

    // Start is called before the first frame update
    void Start()
    {
        puzzlePool = new SelectorPool<string>(puzzles);
    }

    public string LoadPuzzle()
    {
        string puzzleToLoad = puzzlePool.Select();
        PuzzleLoader.LoadPuzzle(puzzleToLoad);

        return puzzleToLoad;
    }
}
