using Assets.Scripts.MainMenu.Characters;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PuzzleLoader : MonoBehaviour
{
    public string puzzleName;
    public Text puzzleText;

    private string originalText;

    private Suspect suspect;

    public Suspect Suspect
    {
        get => suspect;
        set
        {
            suspect = value;
            if (suspect != null)
            {
                puzzleText.text = $"{originalText} ({suspect.cname})";
            }
        }
    }

    public string Puzzle => (Suspect != null && Suspect.Puzzle != null) ? Suspect.Puzzle : puzzleName;

    private void Awake()
    {
        originalText = puzzleText.text;
        Suspect = SuspectManager.GetSuspectByPuzzle(Puzzle);
    }

    private void Start()
    {
        gameObject.SetActive(SuspectManager.IsPuzzleActive(Puzzle));
    }

    public void LoadPuzzle()
    {
        CharacterCreation.Instance.SetCurrentSuspect(Suspect);
        LoadPuzzle(Puzzle);
    }

    public static void LoadPuzzle(string puzzleName)
    {
        SceneManager.LoadScene(puzzleName);
    }
}
