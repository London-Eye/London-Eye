using UnityEngine;
using UnityEngine.SceneManagement;

public class PuzzleLoader : MonoBehaviour
{
    public string puzzleName;

    public void LoadPuzzle() => LoadPuzzle(puzzleName);

    public static void LoadPuzzle(string puzzleName)
    {
        SceneManager.LoadScene(puzzleName);
    }
}
