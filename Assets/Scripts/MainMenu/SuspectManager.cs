using Assets.Scripts.MainMenu.Characters;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SuspectManager : MonoBehaviour
{
    // This is able to persist the suspects references, and restore them in the appropiate instances
    private static readonly Dictionary<int, Suspect> suspectSafe = new Dictionary<int, Suspect>();

    public static Suspect GetSuspectByPuzzle(string puzzle)
    {
        foreach (Suspect suspect in suspectSafe.Values)
        {
            if (suspect.Puzzle == puzzle)
            {
                return suspect;
            }
        }
        return null;
    }

    public Text suspectText;

    private int Id => transform.GetSiblingIndex();

    private Suspect suspect;

    public Suspect Suspect
    {
        get => suspect;
        set
        {
            suspect = value;

            suspectSafe[Id] = value;

            if (suspect != null)
            {
                suspectText.text = suspect.cname;
            }

            gameObject.SetActive(value != null);
        }
    }

    private void Awake()
    {
        if (suspectSafe.TryGetValue(Id, out Suspect suspect))
        {
            Suspect = suspect;
        }
        else
        {
            gameObject.SetActive(Suspect != null);
        }
    }

    public void SelectSuspect()
    {
        CharacterCreation.Instance.SetCurrentSuspect(Suspect);
    }

    public void LoadPuzzle()
    {
        if (Suspect.Puzzle == null)
        {
            string puzzle = FindObjectOfType<PoolPuzzleLoader>().LoadPuzzle();
            Suspect.Puzzle = puzzle;
            PoolPuzzleLoader.ActivePuzzle(puzzle);
        }
        else if (PoolPuzzleLoader.IsPuzzleActive(Suspect.Puzzle))
        {
            PoolPuzzleLoader.LoadPuzzle(Suspect.Puzzle);
        }
    }
}
