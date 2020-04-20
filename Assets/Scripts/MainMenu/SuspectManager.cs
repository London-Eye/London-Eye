﻿using Assets.Scripts.Characters;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Yarn.Unity;

public class SuspectManager : MonoBehaviour, IComparable<SuspectManager>
{
    public const string AccusationSceneName = "Accusation";
    private const string NoEvidenceText = "Sin pruebas";

    public static bool IsInAccusationMenu = false;

    // This is able to persist the suspects references, and restore them in the appropiate instances
    private static readonly Dictionary<int, Suspect> suspectSafe = new Dictionary<int, Suspect>();

    internal static readonly Dictionary<Suspect, PuzzleLoader> activeSuspects = new Dictionary<Suspect, PuzzleLoader>();

    public Image suspectImage;

    public Text suspectText;

    public TMPro.TextMeshProUGUI suspectInfoUp, suspectInfoMiddle, suspectInfoDown;

    private int Id => transform.parent.GetSiblingIndex();

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
                suspectImage.sprite = suspect.Image;

                #region Suspect Info Panel
                if (suspect.EvidencesFound > 2)
                {
                    suspectInfoDown.text = $"Coartada válida: {suspect.HasAlibiAsString}";
                    suspectInfoDown.gameObject.SetActive(true);
                }
                else
                {
                    suspectInfoDown.gameObject.SetActive(false);
                }

                if (suspect.EvidencesFound > 1)
                {
                    suspectInfoMiddle.text = $"Emoción: {suspect.Emotion} la víctima";
                    suspectInfoMiddle.gameObject.SetActive(true);
                }
                else
                {
                    suspectInfoMiddle.gameObject.SetActive(false);
                }

                if (suspect.EvidencesFound > 0)
                {
                    suspectInfoUp.text = $"Relación: {suspect.Relation}";
                    suspectInfoUp.gameObject.SetActive(true);
                }
                else
                {
                    suspectInfoUp.gameObject.SetActive(false);

                    suspectInfoMiddle.text = NoEvidenceText;
                    suspectInfoMiddle.gameObject.SetActive(true);

                    suspectInfoDown.gameObject.SetActive(false);
                } 
                #endregion
            }

            transform.parent.gameObject.SetActive(value != null);
        }
    }

    public Transform PuzzlesView;

    public PuzzleLoader PuzzleButtonPrefab;

    private PuzzleLoader InstantiatePuzzleLoader(Suspect suspect)
    {
        PuzzleLoader puzzleLoader = Instantiate(PuzzleButtonPrefab, PuzzlesView);
        puzzleLoader.Suspect = suspect;

        return puzzleLoader;
    }

    private void Awake()
    {
        if (suspectSafe.TryGetValue(Id, out Suspect suspect))
        {
            Suspect = suspect;

            if (activeSuspects.ContainsKey(suspect))
            {
                activeSuspects[suspect] = InstantiatePuzzleLoader(suspect);
            }
        }
        else
        {
            transform.parent.gameObject.SetActive(Suspect != null);
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
        if (!Suspect.HasFoundAllEvidences)
        {
            if (activeSuspects.ContainsKey(Suspect))
            {
                activeSuspects[Suspect].LoadPuzzle();
            }
            else
            {
                try
                {
                    CharacterCreation.Instance.PoolPuzzleLoader.SelectPuzzle(Suspect);

                    PuzzleLoader puzzleLoader = InstantiatePuzzleLoader(Suspect);
                    activeSuspects[Suspect] = puzzleLoader;

                    puzzleLoader.LoadPuzzle();
                }
                catch (InvalidOperationException ex)
                {
                    Debug.LogWarning(ex);

                    // The puzzle pool is empty. TODO: Warn the user

                }
            }
        }
    }

    public void Accuse()
    {
        AsyncOperation loadSceneOperation = SceneManager.LoadSceneAsync(AccusationSceneName);
        loadSceneOperation.completed += op => FindObjectOfType<DialogueRunner>().startNode = "Acusacion-" + CharacterCreation.Instance.CurrentSuspect.CurrentAccusationState;
    }

    public int CompareTo(SuspectManager other) => Id.CompareTo(other.Id);
}
