using UnityEngine;

public abstract class PuzzleSetter : MonoBehaviour
{
    public bool SetPuzzleOnStart = true;

    // Start is called before the first frame update
    void Start()
    {
        if (SetPuzzleOnStart) TrySetPuzzle();
    }

    public void TrySetPuzzle() => SetPuzzle(CharacterCreation.Instance.PoolPuzzleLoader.GetCurrentPuzzleCombination(GetType()));

    protected abstract void SetPuzzle(int selector);
}
