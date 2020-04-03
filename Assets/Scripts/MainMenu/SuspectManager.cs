using Assets.Scripts.MainMenu.Characters;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SuspectManager : MonoBehaviour
{
    // This is able to persist the suspects references, and restore them in the appropiate instances
    private static readonly Dictionary<int, SuspectManager> suspectManagerSafe = new Dictionary<int, SuspectManager>();

    public Text suspectText;

    private Suspect suspect;

    public Suspect Suspect
    {
        get => suspect;
        set
        {
            suspect = value;

            if (suspect != null)
            {
                suspectText.text = suspect.cname;
            }

            gameObject.SetActive(value != null);
        }
    }

    public string Puzzle { get; private set; }

    private void Awake()
    {
        int id = transform.GetSiblingIndex();

        if (suspectManagerSafe.TryGetValue(id, out SuspectManager suspectManager))
        {
            Suspect = suspectManager.Suspect;
            Puzzle = suspectManager.Puzzle;
        }
        else
        {
            gameObject.SetActive(Suspect != null);
        }

        suspectManagerSafe[id] = this;
    }

    public void SelectSuspect()
    {
        CharacterCreation.Instance.SetCurrentSuspect(Suspect);
    }

    public void LoadPuzzle()
    {
        if (Puzzle == null) { Puzzle = FindObjectOfType<PoolPuzzleLoader>().LoadPuzzle(); }
        else { PuzzleLoader.LoadPuzzle(Puzzle); }
    }
}
