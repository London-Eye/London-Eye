using Assets.Scripts.Characters;
using Assets.Scripts.Common;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Yarn.Unity;

public class PoolPuzzleLoader : MonoBehaviour
{
    public static string CurrentPuzzle { get; private set; }

    private static readonly HashSet<string> activePuzzles = new HashSet<string>();

    public static void ActivePuzzle(string puzzle)
    {
        activePuzzles.Add(puzzle);
        var puzzleType = PuzzleSetterTypes[puzzle];
        CurrentPuzzleCombinations[puzzleType] = CharacterCreation.Instance.PuzzleCombinationPools[puzzleType].Select(); 
    }

    public static bool IsPuzzleActive(string puzzle) => activePuzzles.Contains(puzzle);

    private static readonly Dictionary<string, System.Type> PuzzleSetterTypes = new Dictionary<string, System.Type>()
    {
        { "probetas", typeof(ProbetasGameController) },
        { "MoveTheBlock", typeof(BlockSetter) },
        { "CompletaElCamino", typeof(PipeSetter) },
        { "9-puzzle", typeof(ImageSetter) },
    };

    private static readonly Dictionary<System.Type, int> CurrentPuzzleCombinations = new Dictionary<System.Type, int>();

    internal static int GetCurrentPuzzleCombination(System.Type puzzleType) => CurrentPuzzleCombinations[puzzleType];


    public List<string> puzzles;

    private SelectorPool<string> puzzlePool;

    // Start is called before the first frame update
    void Start()
    {
        puzzlePool = new SelectorPool<string>(puzzles);
        puzzlePool.Fill();
    }

    public string LoadPuzzle()
    {
        string puzzleToLoad = puzzlePool.Select();

        ActivePuzzle(puzzleToLoad);
        LoadPuzzle(puzzleToLoad);

        return puzzleToLoad;
    }

    public static void LoadPuzzle(string puzzleName)
    {
        CurrentPuzzle = puzzleName;
        SceneManager.LoadScene(puzzleName);
    }

    [YarnCommand("CompletePuzzle")]
    public void CompleteCurrentPuzzle()
    {
        bool updateSuspect = false;
        if (CharacterCreation.Instance != null)
        {
            Suspect currentSuspect = CharacterCreation.Instance.CurrentSuspect;
            if (currentSuspect != null) currentSuspect.Puzzle = null;
            else updateSuspect = true;
        }
        else
        {
            updateSuspect = true;
        }

        CompletePuzzle(CurrentPuzzle, updateSuspect, false);
        CurrentPuzzle = null;
    }

    public void CompletePuzzle(string puzzle, bool updateSuspect, bool updatePuzzleLoaders)
    {
        activePuzzles.Remove(puzzle);
        puzzlePool.TryPushAndShuffle(puzzle);

        if (updateSuspect)
        {
            Suspect currentSuspect = SuspectManager.GetSuspectByPuzzle(puzzle);
            if (currentSuspect != null) currentSuspect.Puzzle = null;
        }

        if (updatePuzzleLoaders)
        {
            foreach (PuzzleLoader puzzleLoader in FindObjectsOfType<PuzzleLoader>())
            {
                puzzleLoader.CheckActive();
            }
        }
    }
}
