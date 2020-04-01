using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Assets.Scripts.Dialogue.Texts.Snippets.Sources;

[RequireComponent(typeof(DictionarySnippetSource))]
public class CharacterCreation : MonoBehaviour
{
    [SerializeField] public CharacterStats maleCharacterStats;
    [SerializeField] public CharacterStats femaleCharacterStats;

    public int suspectsGiven = 5;
    public string suspectKey, victimKey, murdererKey;

    private List<int> mNames = new List<int>(), fNames = new List<int>();
    private List<int> mRelation = new List<int>(), fRelation = new List<int>();

    private DictionarySnippetSource characterDictionary;

    void Start()
    {
        DontDestroyOnLoad(this);
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

                if(isMale)
                {
                    name = rnd.Next(0, 9);
                    relation = rnd.Next(0, 6);
                    while (mNames.Contains(name) || mRelation.Contains(relation))
                    {
                        name = rnd.Next(0, 9);
                        relation = rnd.Next(0, 6);
                    }

                    mNames.Add(name);
                    if (relation > 1)
                    {
                        mRelation.Add(relation);
                    }
                } else
                {
                    name = rnd.Next(0, 9);
                    relation = rnd.Next(0, 6);
                    while (fNames.Contains(name) || fRelation.Contains(relation))
                    {
                        name = rnd.Next(0, 9);
                        relation = rnd.Next(0, 6);
                    }

                    fNames.Add(name);
                    if (relation > 1)
                    {
                        fRelation.Add(relation);
                    }
                }

                emotion = rnd.Next(0, 7);
                hasAlibi = rnd.NextDouble() > 0.5;

                current = InitializeCharacter(isMale, name, relation, emotion, hasAlibi);

                characterDictionary.Snippets[suspectKey] = current;
            }
            
        }

        isMale = rnd.NextDouble() > 0.5;

        if (isMale)
        {
            name = rnd.Next(0, 9);
            relation = rnd.Next(0, 6);
            while (mNames.Contains(name) || mRelation.Contains(relation))
            {
                name = rnd.Next(0, 9);
                relation = rnd.Next(0, 6);
            }

            mNames.Add(name);
            if (relation > 1)
            {
                mRelation.Add(relation);
            }
        }
        else
        {
            name = rnd.Next(0, 9);
            relation = rnd.Next(0, 6);
            while (fNames.Contains(name) || fRelation.Contains(relation))
            {
                name = rnd.Next(0, 9);
                relation = rnd.Next(0, 6);
            }

            fNames.Add(name);
            if (relation > 1)
            {
                fRelation.Add(relation);
            }
        }

        emotion = rnd.Next(0, 7);
        hasAlibi = rnd.NextDouble() > 0.5;

        current = InitializeCharacter(isMale, name, relation, emotion, hasAlibi);

        characterDictionary.Snippets[victimKey] = current;

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

}
