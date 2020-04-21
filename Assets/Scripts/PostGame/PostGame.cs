using Assets.Scripts.Characters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PostGame : MonoBehaviour
{
    [SerializeField] private GameObject LoseCanvas;
    [SerializeField] private GameObject WinCanvas;
    // Start is called before the first frame update
    void Start()
    {
        Suspect sus = CharacterCreation.Instance.CurrentSuspect;
        switch (sus.CurrentAccusationState) {
            case Suspect.AccusationState.Criminal:
                WinCanvas.SetActive(true);
                break;
            default:
                LoseCanvas.SetActive(true);
                break;
        }
    }

    public void GoToTittle() {
        GameObject.Destroy(GameObject.Find("GameManager"));
        SceneManager.LoadScene("Portada");
    }
    
}
