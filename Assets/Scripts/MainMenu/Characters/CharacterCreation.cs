using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Dialogue.Texts.Snippets.Sources;

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

    public int suspectsGiven = 5;
    public string suspectKey, victimKey, murdererKey, randomNameKey;

    private readonly List<int> mNames = new List<int>(), fNames = new List<int>();
    private readonly List<int> mRelation = new List<int>(), fRelation = new List<int>();

    private DictionarySnippetSource characterDictionary;
    private PoolSnippetSource namePool;

    private CharacterCreation() { }

    void Awake()
    {
        MakeSingleton();
    }

    void Start()
    {
        characterDictionary = GetComponent<DictionarySnippetSource>();

        int generateSuspects = suspectsGiven + 2;
        System.Random rnd = new System.Random();

        bool isMale, hasAlibi;
        int name, relation, emotion;

        Character current;

        for (int i = 0; i < generateSuspects; i++)
        {

            if (i == 0)
            {
                isMale = rnd.NextDouble() > 0.5;
                name = rnd.Next(0, 9);
                relation = rnd.Next(0, 6);
                emotion = rnd.Next(0, 7);
                hasAlibi = false;

                current = InitializeCharacter(isMale, name, relation, emotion, hasAlibi);

                characterDictionary.Snippets[murdererKey] = current;

                if(isMale)
                {
                    mNames.Add(name);
                    if(relation > 1)
                    {
                        mRelation.Add(relation);
                    }
                } else
                {
                    fNames.Add(name);
                    if (relation > 1)
                    {
                        fRelation.Add(relation);
                    }
                }

            } else
            {
                isMale = rnd.NextDouble() > 0.5;

                name = SetName(rnd, isMale);
                relation = SetRelation(rnd, isMale);

                emotion = rnd.Next(0, 7);
                hasAlibi = rnd.NextDouble() > 0.5;

                current = InitializeCharacter(isMale, name, relation, emotion, hasAlibi);

                characterDictionary.Snippets[suspectKey] = current;
            }
            
        }

        isMale = rnd.NextDouble() > 0.5;

        name = SetName(rnd, isMale);
        relation = SetRelation(rnd, isMale);

        emotion = rnd.Next(0, 7);
        hasAlibi = rnd.NextDouble() > 0.5;

        current = InitializeCharacter(isMale, name, relation, emotion, hasAlibi);

        characterDictionary.Snippets[victimKey] = current;

        FillNamePool();

    }

    private Character InitializeCharacter(bool isMale, int name, int relation, int emotion, bool hasAlibi)
    {
        Character current = ScriptableObject.CreateInstance<Character>();

        current.isMale = isMale;

        if(isMale)
        {
            current.cname = maleCharacterStats.characterName[name];
            current.relation = maleCharacterStats.relation[relation];
            current.emotion = maleCharacterStats.emotion[emotion];
        } else
        {
            current.cname = femaleCharacterStats.characterName[name];
            current.relation = femaleCharacterStats.relation[relation];
            current.emotion = femaleCharacterStats.emotion[emotion];
        }

        current.hasAlibi = hasAlibi;

        return current;
    }

    private int SetName(System.Random rnd, bool isMale)
    {
        int name;
        if (isMale)
        {
            name = rnd.Next(0, 9);
            while (mNames.Contains(name))
            {
                name = rnd.Next(0, 9);
            }

            mNames.Add(name);
        }
        else
        {
            name = rnd.Next(0, 9);
            while (fNames.Contains(name))
            {
                name = rnd.Next(0, 9);
            }

            fNames.Add(name);
        }
        return name;
    }

    private int SetRelation(System.Random rnd, bool isMale)
    {
        int relation;
        if (isMale)
        {
            relation = rnd.Next(0, 6);
            while (mRelation.Contains(relation))
            {
                relation = rnd.Next(0, 6);
            }

            if (relation > 1)
            {
                mRelation.Add(relation);
            }
        }
        else
        {
            relation = rnd.Next(0, 6);
            while (fRelation.Contains(relation))
            {

                relation = rnd.Next(0, 6);
            }

            if (relation > 1)
            {
                fRelation.Add(relation);
            }
        }
        return relation;
    }

    private void FillNamePool()
    {
        namePool = GetComponent<PoolSnippetSource>();
        
        for(int i = 0; i < maleCharacterStats.characterName.Length; i++)
        {
            if(!mRelation.Contains(i))
            {
                namePool.Snippets[randomNameKey] = maleCharacterStats.characterName[i];
            }
        }

        for (int i = 0; i < femaleCharacterStats.characterName.Length; i++)
        {
            if (!fRelation.Contains(i))
            {
                namePool.Snippets[randomNameKey] = femaleCharacterStats.characterName[i];
            }
        }
    }
}
