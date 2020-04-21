using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PostGameLauncher : MonoBehaviour
{
    // Start is called before the first frame update
    public void loadPostGame() {
        SceneManager.LoadScene("PostGame");
    }
}
