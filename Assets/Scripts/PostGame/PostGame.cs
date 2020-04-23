using Assets.Scripts.Characters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PostGame : MonoBehaviour
{
    [SerializeField] private AudioClip win;
    [SerializeField] private AudioClip lose;
    [SerializeField] private GameObject LoseCanvas;
    [SerializeField] private GameObject WinCanvas;
    // Start is called before the first frame update
    void Start()
    {
        GameObject.Find("GameObject").GetComponent<AudioSource>().Stop();
        GameObject.Find("GameManager").GetComponent<AudioSource>().Stop();
        Suspect sus = CharacterCreation.Instance.CurrentSuspect;
        switch (sus.Ending) {
            case 1:
                WinCanvas.SetActive(true);
                GameObject.Find("GameObject").GetComponent<AudioSource>().clip = win;
                GameObject.Find("GameObject").GetComponent<AudioSource>().Play();
                break;
            default:
                LoseCanvas.SetActive(true);
                GameObject.Find("GameObject").GetComponent<AudioSource>().clip = lose;
                GameObject.Find("GameObject").GetComponent<AudioSource>().Play();
                break;
        }
    }

    public void GoToTittle() {
        GameObject.Destroy(GameObject.Find("GameManager"));
        SceneManager.LoadScene("Portada");
    }
    
}
