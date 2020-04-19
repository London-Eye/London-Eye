using Assets.Scripts.Characters;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PuzzleLoader : MonoBehaviour
{
    public Text puzzleText;
    public Image puzzleImage;

    private Suspect suspect;

    public Suspect Suspect
    {
        get => suspect;
        set
        {
            suspect = value;
            if (suspect != null)
            {
                puzzleText.text = $"{suspect.Puzzle} ({suspect.cname})";
                puzzleImage.sprite = CharacterCreation.Instance.PoolPuzzleLoader.GetPuzzleMiniature(suspect.Puzzle);
            }
        }
    }

    public void LoadPuzzle()
    {
        CharacterCreation.Instance.SetCurrentSuspect(Suspect);
        SceneManager.LoadScene(Suspect.Puzzle);
    }
}
