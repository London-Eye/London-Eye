using Assets.Scripts.Characters;
using Assets.Scripts.Common.Pools;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class PoolPuzzleLoader : MonoBehaviour
{
    public const int PuzzleLimit = 12;

    private static readonly Dictionary<string, System.Type> PuzzleSetterTypes = new Dictionary<string, System.Type>()
    {
        { "probetas", typeof(ProbetasGameController) },
        { "MoveTheBlock", typeof(BlockSetter) },
        { "CompletaElCamino", typeof(PipeSetter) },
        { "9-puzzle", typeof(ImageSetter) },
    };

    private readonly Dictionary<System.Type, int> CurrentPuzzleCombinations = new Dictionary<System.Type, int>();

    internal int GetCurrentPuzzleCombination(System.Type puzzleType) => CurrentPuzzleCombinations[puzzleType];

    public List<string> puzzles;
    public List<string> puzzleNames;
    public List<Sprite> puzzleMiniatures;

    internal Sprite GetPuzzleMiniature(string puzzle) => puzzleMiniatures[puzzles.IndexOf(puzzle)];

    internal string GetPuzzleName(string puzzle) => puzzleNames[puzzles.IndexOf(puzzle)];

    internal readonly Dictionary<Suspect, PuzzleLoader> activeSuspects = new Dictionary<Suspect, PuzzleLoader>();

    private SelectorPool<string> puzzlePool;

    // Start is called before the first frame update
    void Start()
    {
        puzzlePool = new SelectorPool<string>(puzzles) { AutoRefill = true, SelectLimit = PuzzleLimit };
    }
        
    public void SelectPuzzle(Suspect suspect)
    {
        string puzzleToLoad = puzzlePool.Select();
        suspect.Puzzle = puzzleToLoad;

        var puzzleType = PuzzleSetterTypes[puzzleToLoad];
        CurrentPuzzleCombinations[puzzleType] = CharacterCreation.Instance.PuzzleCombinationPools[puzzleType].Select();
    }

    [YarnCommand("CompletePuzzle")]
    public void CompleteCurrentPuzzle()
        => CompletePuzzle(CharacterCreation.Instance.CurrentSuspect);

    public void CompletePuzzle(Suspect suspect)
    {
        suspect.Puzzle = null;
        if (activeSuspects.ContainsKey(suspect))
        {
            Destroy(activeSuspects[suspect]);
            activeSuspects.Remove(suspect);
        }
    }
}
