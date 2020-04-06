using Assets.Scripts.MainMenu.Characters;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Yarn.Unity;

public class SuspectManager : MonoBehaviour, IComparable<SuspectManager>
{
    public const string AccusationSceneName = "Accusation";

    public static bool IsInAccusationMenu = false;

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

    public void SetIsInAccusationMenu(bool value) => IsInAccusationMenu = value;

    public void LoadPuzzleOrAccuse()
    {
        if (IsInAccusationMenu) Accuse();
        else LoadPuzzle();
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

    public void Accuse()
    {
        AsyncOperation loadSceneOperation = SceneManager.LoadSceneAsync(AccusationSceneName);
        loadSceneOperation.completed += op => FindObjectOfType<DialogueRunner>().startNode = "Acusacion-" + CharacterCreation.Instance.CurrentSuspect.AccusationState;
    }

    public int CompareTo(SuspectManager other) => Id.CompareTo(other.Id);
}
