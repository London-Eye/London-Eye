using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Common;
using UnityEngine.SceneManagement;
using Yarn.Unity;
using Assets.Scripts.Characters;
using UnityEngine.Events;
using Assets.Scripts.Dialogue.Variables.Storages;
using Assets.Scripts.Puzzles;
using Assets.Scripts.Common.Pools;

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
        randomPoolSource.ResetToDefaultsForce();
        randomTextPoolSource.ResetToDefaultsForce();

        InitializePools();

        CreateVictim();

        VariableStorageGroup variableStorageWithFallback = gameObject.AddComponent<VariableStorageGroup>();
        variableStorageWithFallback.sources = new List<VariableStorageBehaviour>() { VariableStorage, FallbackVariableStorage };

        if (VariableStorageWithFallback != null) Destroy(VariableStorageWithFallback.gameObject);
        VariableStorageWithFallback = variableStorageWithFallback;

        OnNewGame.Invoke();
    }

    // maxNumberOfSuspects <= min(numberOfNames, numberOfRelations, numberOfEmotions, numberOfImages)
    public const int maxNumberOfSuspects = 6, numberOfNames = 10, numberOfRelations = 7, numberOfEmotions = 8, numberOfImages = 6;

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

    private Victim victim;
    public Victim Victim
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

    public VariableStorageGroup VariableStorage;
    public VariableStorageBehaviour VariableStorageWithFallback { get; private set; }

    public VariableStorageBehaviour FallbackVariableStorage;

    public SimpleAccessibleVariableStorage characterVariableStorage;

    public PoolPuzzleLoader PoolPuzzleLoader { get; private set; }

    private SelectorPool<int> mNames, fNames;
    private SelectorPool<int> mRelation, fRelation;
    private SelectorPool<int> mEmotion, fEmotion;
    private SelectorPool<int> mImages, fImages;

    public PoolVariableStorage randomPoolSource;
    public TextAssetPoolVariableStorage randomTextPoolSource;

    internal Dictionary<System.Type, SelectorPool<int>> PuzzleCombinationPools { get; set; }

    void Awake()
    {
        MakeSingleton();
    }

    private void Start()
    {
        PoolPuzzleLoader = GetComponent<PoolPuzzleLoader>();
    }

    public void CreateVictim() => Victim = InitializeCharacter<Victim>();

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
        for (int i = 0; i < suspectManagers.Length; i++)
        {
            suspectManagers[i].Suspect = i < suspectsShuffled.Length ? suspectsShuffled[i] : null;
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
        // Suspect stats

        InitializeGenderedPool(numberOfNames, out mNames, out fNames);
        InitializeGenderedPool(numberOfRelations, out mRelation, out fRelation);
        InitializeGenderedPool(numberOfEmotions, out mEmotion, out fEmotion);
        InitializeGenderedPool(numberOfImages, out mImages, out fImages);

        // Puzzle combinations
        PuzzleCombinationPools = new Dictionary<System.Type, SelectorPool<int>>();
        InitializePuzzleCombinationPool(PuzzleCombinations.Probetas, typeof(ProbetasGameController));
        InitializePuzzleCombinationPool(PuzzleCombinations.MoveTB, typeof(BlockSetter));
        InitializePuzzleCombinationPool(PuzzleCombinations.CompletaEC, typeof(PipeSetter));
        InitializePuzzleCombinationPool(PuzzleCombinations.NinePuzzle, typeof(ImageSetter));
    }

    private void InitializePuzzleCombinationPool(int numberOfCombinations, System.Type puzzleType)
    {
        InitializePool(numberOfCombinations, out SelectorPool<int> combinationPool, false, true);
        PuzzleCombinationPools.Add(puzzleType, combinationPool);
    }

    private static void InitializeGenderedPool(int numberOfElements, out SelectorPool<int> malePool, out SelectorPool<int> femalePool)
    {
        InitializePool(numberOfElements, out malePool);
        InitializePool(numberOfElements, out femalePool);
    }

    private static void InitializePool(int numberOfElements, out SelectorPool<int> pool, bool autoRefill = true, bool fillOnStart = false)
    {
        HashSet<int> elements = new HashSet<int>();
        Utilities.AddIntRange(elements, 0, numberOfElements);

        pool = new SelectorPool<int>(elements) { AutoRefill = autoRefill };
        if (fillOnStart) pool.Fill();
    }

    private T InitializeCharacter<T>() where T : Character
    {
        bool isMale = Utilities.RandomBool();
        int name = isMale ? mNames.Select() : fNames.Select();

        return InitializeCharacter<T>(isMale, name);
    }

    private T InitializeCharacter<T>(bool isMale, int name) where T : Character
    {
        T character = ScriptableObject.CreateInstance<T>();
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
        int emotion = isMale ? mEmotion.Select() : fEmotion.Select();
        int image = isMale ? mImages.Select() : fImages.Select();

        return InitializeSuspect(isMale, name, relation, emotion, hasAlibi, image);
    }

    private Suspect InitializeSuspect(bool isMale, int name, int relation, int emotion, bool hasAlibi, int image)
    {
        Suspect suspect = ScriptableObject.CreateInstance<Suspect>();

        suspect.IsMale = isMale;

        FillSuspectWithStats(suspect, isMale ? maleCharacterStats : femaleCharacterStats, name, relation, emotion, image);

        suspect.HasAlibi = hasAlibi;

        return suspect;
    }

    private void FillSuspectWithStats(Suspect suspect, CharacterStats stats, int nameIndex, int relationIndex, int emotionIndex, int imageIndex)
    {
        suspect.cname = stats.characterName[nameIndex];
        suspect.Relation = stats.relation[relationIndex];
        suspect.Emotion = stats.emotion[emotionIndex];
        suspect.RelationIndexCriminal = relationIndex;
        suspect.EmotionIndexCriminal = emotionIndex;
        suspect.Image = stats.images[imageIndex];
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

        randomPoolSource.SelectorPools[randomNameKey] = randomNamePool;
    }
}
