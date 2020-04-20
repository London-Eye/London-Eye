using Assets.Scripts.Common;
using UnityEngine;

public abstract class PuzzleSetter : MonoBehaviour
{
    public DialogueController dialogueController;

    public bool SetPuzzleOnStart = true;

    // Start is called before the first frame update
    void Start()
    {
        if (SetPuzzleOnStart) TrySetPuzzle();
    }

    public void TrySetPuzzle() => SetPuzzle(CharacterCreation.Instance.PoolPuzzleLoader.GetCurrentPuzzleCombination(GetType()));

    protected abstract void SetPuzzle(int selector);
}
