using Assets.Scripts.Characters;
using Assets.Scripts.Common;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Yarn.Unity;

public class PoolPuzzleLoader : MonoBehaviour
{
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
    public List<Sprite> puzzleMiniatures;

    internal Sprite GetPuzzleMiniature(string puzzleName)
    {
        return puzzleMiniatures[puzzles.IndexOf(puzzleName)];
    }

    private SelectorPool<string> puzzlePool;

    // Start is called before the first frame update
    void Start()
    {
        puzzlePool = new SelectorPool<string>(puzzles) { AutoRefill = true };
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
        Destroy(SuspectManager.activeSuspects[suspect]);
        SuspectManager.activeSuspects.Remove(suspect);
    }
}
