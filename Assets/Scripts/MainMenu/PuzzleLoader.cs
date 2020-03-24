using UnityEngine;
using UnityEngine.SceneManagement;

public class PuzzleLoader : MonoBehaviour
{
    public string puzzleName;

    public void LoadPuzzle()
    {
        SceneManager.LoadScene(puzzleName);
    }
}
