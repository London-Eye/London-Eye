using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Common;
using UnityEngine.SceneManagement;
using Yarn.Unity;
using Assets.Scripts.Characters;
using UnityEngine.Events;
using Assets.Scripts.Dialogue.Variables.Storages;

[RequireComponent(typeof(PoolVariableStorage))]
public class CharacterCreation : MonoBehaviour
{
    public const string InitialPuzzle = "CardGame";

    public UnityEvent OnNewGame;

    public static CharacterCreation Instance { get; private set; }

    private void MakeSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);

            // When the start of game dialogue ends, load the initial puzzle
            FindObjectOfType<DialogueUI>().onDialogueEnd.AddListener(() => SceneManager.LoadScene(InitialPuzzle));

            FindObjectOfType<DialogueRunner>().startAutomatically = true;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    [YarnCommand("StartNewGame")]
    public void NewGame()
    {
        characterVariableStorage.ResetToDefaultsForce();

        InitializePools();

        CreateVictim();

        randomNamePoolSource.ResetToDefaultsForce();

        VariableStorageGroup variableStorage = gameObject.AddComponent<VariableStorageGroup>();
        variableStorage.sources = new List<VariableStorageBehaviour>() { characterVariableStorage, randomNamePoolSource };

        VariableStorageGroup variableStorageWithFallback = gameObject.AddComponent<VariableStorageGroup>();
        variableStorageWithFallback.sources = new List<VariableStorageBehaviour>() { variableStorage, FallbackVariableStorage };

        if (VariableStorage != null) Destroy(VariableStorage.gameObject);
        VariableStorage = variableStorage;
        
        if (VariableStorageWithFallback != null) Destroy(VariableStorageWithFallback.gameObject);
        VariableStorageWithFallback = variableStorageWithFallback;

        OnNewGame.Invoke();
    }

    private void LoadGame()
    {
        // TODO: Add Game Loading
    }

    private void SaveGame()
    {
        // TODO: Add Game Saving
    }

    // maxNumberOfSuspects = min(numberOfNames, numberOfRelations, numberOfEmotions);
    public const int maxNumberOfSuspects = 6, numberOfNames = 9, numberOfRelations = 6, numberOfEmotions = 7;

    public const string MainMenuMenusGameObjectName = "Menus";

    private int numberOfSuspects;
    public int NumberOfSuspects
    {
        get => numberOfSuspects;
        set
        {
            numberOfSuspects = value < maxNumberOfSuspects ? value : maxNumberOfSuspects;
            VariableStorageWithFallback.SetValueNoLeading(numberOfSuspectsKey, numberOfSuspects);
        }
    }

    private Character victim;
    public Character Victim
    {
        get => victim;
        private set
        {
            victim = value;
            characterVariableStorage.SetValueNoLeading(victimKey, objectValue: value);
        }
    }

    public Suspect Murderer { get; private set; }

    public Suspect CurrentSuspect { get; private set; }

    public CharacterStats maleCharacterStats;
    public CharacterStats femaleCharacterStats;

    public string suspectKey, victimKey, murdererKey, randomNameKey, numberOfSuspectsKey;

    private HashSet<Suspect> suspects;
    public IReadOnlyCollection<Suspect> Suspects => suspects;

    public VariableStorageBehaviour VariableStorage { get; private set; }
    public VariableStorageBehaviour VariableStorageWithFallback { get; private set; }

    public VariableStorageBehaviour FallbackVariableStorage;

    public SimpleAccessibleVariableStorage characterVariableStorage;

    public PoolPuzzleLoader PoolPuzzleLoader { get; private set; }

    private SelectorPool<int> mNames, fNames;
    private SelectorPool<int> mRelation, fRelation;
    public PoolVariableStorage randomNamePoolSource;

    void Awake()
    {
        MakeSingleton();
    }

    private void Start()
    {
        PoolPuzzleLoader = GetComponent<PoolPuzzleLoader>();
    }

    public void CreateVictim() => Victim = InitializeCharacter();

    public void CreateSuspects()
    {
        SuspectManager[] suspectManagers = GameObject.Find(MainMenuMenusGameObjectName)
                                                     .GetComponentsInChildren<SuspectManager>(true);

        if (NumberOfSuspects > suspectManagers.Length)
        {
            // If there are not enough managers for all the suspects, log a warning, and use the maximum space available
            Debug.LogWarning($"No space for all suspects ({NumberOfSuspects}). Using max space instead ({suspectManagers.Length})");
            numberOfSuspects = suspectManagers.Length;
        }

        // Sort the managers by its position
        System.Array.Sort(suspectManagers);

        suspects = new HashSet<Suspect>();

        // Create the murderer
        Suspect murderer = InitializeSuspect(hasAlibi: false);
        Murderer = murderer;
        characterVariableStorage.SetValueNoLeading(murdererKey, objectValue: murderer);
        suspects.Add(murderer);

        // Create other suspects
        for (int i = 0; i < NumberOfSuspects - 1; i++)
        {
            Suspect suspect = InitializeSuspect();
            suspects.Add(suspect);
        }

        // Shuffle the suspects and put them in the scene
        Suspect[] suspectsShuffled = suspects.GetShuffle();
        for (int i = 0; i < NumberOfSuspects; i++)
        {
            suspectManagers[i].Suspect = suspectsShuffled[i];
        }

        // If the victim wasn't created yet, do it now
        if (Victim == null) CreateVictim();

        // Fill the remaining names in a pool as random radiant ones
        FillNamePool();
    }

    public void SetCurrentSuspect(Suspect suspect)
    {
        if (suspects.Contains(suspect))
        {
            SetCurrentSuspectImpl(suspect);
        }
        else
        {
            throw new System.ArgumentException($"The suspect must be in the {nameof(Suspects)} list");
        }
    }

    private void SetCurrentSuspectImpl(Suspect suspect)
    {
        CurrentSuspect = suspect;
        characterVariableStorage.SetValueNoLeading(suspectKey, objectValue: suspect);
    }

    private void InitializePools()
    {
        InitializePool(numberOfNames, out mNames, out fNames);
        InitializePool(numberOfRelations, out mRelation, out fRelation);
    }

    private static void InitializePool(int numberOfElements, out SelectorPool<int> malePool, out SelectorPool<int> femalePool)
    {
        HashSet<int> elements = new HashSet<int>();
        Utilities.AddIntRange(elements, 0, numberOfElements);

        malePool = new SelectorPool<int>(elements) { AutoRefill = true };
        femalePool = new SelectorPool<int>(elements) { AutoRefill = true };
    }

    private Character InitializeCharacter()
    {
        bool isMale = Utilities.RandomBool();
        int name = isMale ? mNames.Select() : fNames.Select();

        return InitializeCharacter(isMale, name);
    }

    private Character InitializeCharacter(bool isMale, int name)
    {
        Character character = ScriptableObject.CreateInstance<Character>();
        character.IsMale = isMale;
        character.cname = (isMale ? maleCharacterStats : femaleCharacterStats).characterName[name];
        return character;
    }

    private Suspect InitializeSuspect()
    {
        bool hasAlibi = Utilities.RandomBool();
        return InitializeSuspect(hasAlibi);
    }

    private Suspect InitializeSuspect(bool hasAlibi)
    {
        bool isMale = Utilities.RandomBool();
        int name = isMale ? mNames.Select() : fNames.Select();
        int relation = isMale ? mRelation.Select() : fRelation.Select();
        int emotion = Random.Range(0, numberOfEmotions);

        return InitializeSuspect(isMale, name, relation, emotion, hasAlibi);
    }

    private Suspect InitializeSuspect(bool isMale, int name, int relation, int emotion, bool hasAlibi)
    {
        Suspect suspect = ScriptableObject.CreateInstance<Suspect>();

        suspect.IsMale = isMale;

        FillSuspectWithStats(suspect, isMale ? maleCharacterStats : femaleCharacterStats, name, relation, emotion);

        suspect.HasAlibi = hasAlibi;

        return suspect;
    }

    private void FillSuspectWithStats(Suspect suspect, CharacterStats stats, int nameIndex, int relationIndex, int emotionIndex)
    {
        suspect.cname = stats.characterName[nameIndex];
        suspect.Relation = stats.relation[relationIndex];
        suspect.Emotion = stats.emotion[emotionIndex];
    }

    private void FillNamePool()
    {
        SelectorPool<object> randomNamePool = new SelectorPool<object>() { AutoRefill = true };

        while (mNames.Count > 0)
        {
            randomNamePool.Pool.Add(maleCharacterStats.characterName[mNames.Select()]);
        }

        while (fNames.Count > 0)
        {
            randomNamePool.Pool.Add(femaleCharacterStats.characterName[fNames.Select()]);
        }

        randomNamePoolSource.SelectorPools[randomNameKey] = randomNamePool;
    }
}
