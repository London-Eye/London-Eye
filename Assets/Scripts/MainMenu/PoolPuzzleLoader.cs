﻿using Assets.Scripts.Common;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Yarn.Unity;

public class PoolPuzzleLoader : MonoBehaviour
{
    public static string CurrentPuzzle { get; private set; }

    private static readonly HashSet<string> activePuzzles = new HashSet<string>();

    public static void ActivePuzzle(string puzzle) => activePuzzles.Add(puzzle);

    public static bool IsPuzzleActive(string puzzle) => activePuzzles.Contains(puzzle);


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
        LoadPuzzle(puzzleToLoad);

        return puzzleToLoad;
    }

    public static void LoadPuzzle(string puzzleName)
    {
        CurrentPuzzle = puzzleName;
        SceneManager.LoadScene(puzzleName);
    }

    [YarnCommand("CompletePuzzle")]
    public static void CompleteCurrentPuzzle()
    {
        activePuzzles.Remove(CurrentPuzzle);
        CurrentPuzzle = null;
    }
}