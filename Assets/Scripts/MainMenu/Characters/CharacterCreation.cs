using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Dialogue.Texts.Snippets.Sources;
using Assets.Scripts.Common;
using System.Linq;

[RequireComponent(typeof(DictionarySnippetSource), typeof(PoolSnippetSource))]
public class CharacterCreation : MonoBehaviour
{
    public static CharacterCreation Instance { get; private set; }

    private void MakeSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public CharacterStats maleCharacterStats;
    public CharacterStats femaleCharacterStats;

    // Max suspects must always be 'min(numberOfNames, numberOfRelations, numberOfEmotions) - 1'
    [Range(2, 5)]
    public int suspectsGiven = 5;

    public string suspectKey, victimKey, murdererKey, randomNameKey;

    public const int numberOfNames = 9, numberOfRelations = 6, numberOfEmotions = 7;

    private readonly HashSet<Character> suspects = new HashSet<Character>();
    public IReadOnlyCollection<Character> Suspects => suspects;

    private SelectorPool<int> mNames, fNames;
    private SelectorPool<int> mRelation, fRelation;

    private DictionarySnippetSource characterDictionary;
    private PoolSnippetSource randomNamePoolSource;

    private CharacterCreation() { }

    void Awake()
    {
        MakeSingleton();
    }

    void Start()
    {
        characterDictionary = GetComponent<DictionarySnippetSource>();

        InitializePools();

        // Create the murderer
        characterDictionary.Snippets[murdererKey] = InitializeCharacter(hasAlibi: false);

        // Create other suspects
        for (int i = 0; i < suspectsGiven - 1; i++)
        {
            suspects.Add(InitializeCharacter());
        }

        // By default, set the current suspect to the first one
        // TODO: When the selecting suspect system is done, this should probably be removed to improve efficiency
        SetCurrentSuspectImpl(suspects.First());

        // Create the victim
        characterDictionary.Snippets[victimKey] = InitializeCharacter();

        // Fill the remaining names in a pool as random radiant ones
        FillNamePool();
    }

    public void SetCurrentSuspect(Character suspect)
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

    private void SetCurrentSuspectImpl(Character suspect) => characterDictionary.Snippets[suspectKey] = suspect;

    private void InitializePools()
    {
        InitializePool(numberOfNames, out mNames, out fNames);
        InitializePool(numberOfRelations, out mRelation, out fRelation);
    }

    private static void InitializePool(int numberOfElements, out SelectorPool<int> malePool, out SelectorPool<int> femalePool)
    {
        List<int> elements = new List<int>();
        Utilities.AddIntRange(elements, 0, numberOfElements);

        malePool = new SelectorPool<int>(elements);
        femalePool = new SelectorPool<int>(elements);
    }

    private Character InitializeCharacter()
    {
        bool hasAlibi = Utilities.RandomBool();
        return InitializeCharacter(hasAlibi);
    }

    private Character InitializeCharacter(bool hasAlibi)
    {
        bool isMale = Utilities.RandomBool();
        int name = isMale ? mNames.Select() : fNames.Select();
        int relation = isMale ? mRelation.Select() : fRelation.Select();
        int emotion = Random.Range(0, numberOfEmotions);

        return InitializeCharacter(isMale, name, relation, emotion, hasAlibi);
    }

    private Character InitializeCharacter(bool isMale, int name, int relation, int emotion, bool hasAlibi)
    {
        Character current = ScriptableObject.CreateInstance<Character>();

        current.isMale = isMale;

        FillCharacterWithStats(current, isMale ? maleCharacterStats : femaleCharacterStats, name, relation, emotion);

        current.hasAlibi = hasAlibi;

        return current;
    }

    private void FillCharacterWithStats(Character character, CharacterStats stats, int nameIndex, int relationIndex, int emotionIndex)
    {
        character.cname = stats.characterName[nameIndex];
        character.relation = stats.relation[relationIndex];
        character.emotion = stats.emotion[emotionIndex];
    }

    private void FillNamePool()
    {
        randomNamePoolSource = GetComponent<PoolSnippetSource>();

        SelectorPool<object> randomNamePool = new SelectorPool<object>();
        
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
